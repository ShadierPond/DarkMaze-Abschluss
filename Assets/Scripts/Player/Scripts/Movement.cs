using System.Collections;
using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private DefaultControls _controls;
        
        [Header("General Settings")]
        [SerializeField] private float speedMultiplier = 10;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float drag = 0.5f;
        private float _desiredSpeed;
        private float _lastDesiredSpeed;
        private Rigidbody _rigidbody;
        private Vector2 _input;
        private Transform _orientation;
        public bool isGrounded;
        
        [Header("Movement Settings")]
        [SerializeField] private float walkingSpeed;
        private Vector3 _moveDirection;
        
        [Header("Crouch Settings")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private bool holdToCrouch;
        [SerializeField] private float crouchHeight;
        private float _currentHeight;
        private bool _isCrouching;
        private CapsuleCollider _collider;
         
        [Header("Running Settings")]
        [SerializeField] private float runningSpeed;
        [SerializeField] private bool holdToRun;
        private bool _isRunning;
        
        [Header("Jumping Settings")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float airSpeedMultiplier = 0.5f;
        [SerializeField] private bool holdToJump;
        [SerializeField, Tooltip("Debug")] private bool isJumping;
        private bool _jumpInput;
        private bool _startJump;
        private float _jumpGroundCheckTimer;
        private const float JumpGroundCheckDelay = 0.2f;

        [Header("Animation Settings")]
        [SerializeField] private Animator animator;
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int IsCrouching = Animator.StringToHash("isCrouching");

        /// <summary>
        /// This is called when the script instance is being loaded.
        /// This creates a new instance of the DefaultControls class and enables it.
        /// It sets the the input actions to the variables and adds a listener to the actions.
        /// </summary>
        private void OnEnable()
        {
            _controls = new DefaultControls();
            _controls.Enable();
            _controls.Player.Move.performed += ctx => _input = ctx.ReadValue<Vector2>();
            _controls.Player.Move.canceled += ctx => _input = Vector2.zero;
            _controls.Player.Sprint.performed += ctx => _isRunning = holdToRun ? ctx.ReadValueAsButton() : ctx.ReadValueAsButton() ? !_isRunning : _isRunning;
            if(holdToRun)
                _controls.Player.Sprint.canceled += ctx => _isRunning = false;
            _controls.Player.Jump.performed += ctx => _jumpInput = ctx.ReadValueAsButton();
            _controls.Player.Jump.canceled += ctx => _jumpInput = false;
            _controls.Player.Crouch.performed += ctx => _isCrouching = holdToCrouch ? ctx.ReadValueAsButton() : ctx.ReadValueAsButton() ? !_isCrouching : _isCrouching;
            if(holdToCrouch)
                _controls.Player.Crouch.canceled += ctx => _isCrouching = false;
        }
        
        /// <summary>
        /// This is called when the script instance is being disabled.
        /// This disables the controls.
        /// </summary>
        private void OnDisable()
        {
            _controls.Disable();
        }

        /// <summary>
        /// This sets the orientation variable to the first child of the player.
        /// It is called before Start.
        /// </summary>
        private void Awake()
        {
            _orientation = transform.GetChild(0);
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _currentHeight = _collider.height;
        }
        
        /// <summary>
        /// Called once per frame.
        /// Calls the methods that are needed to update the player.
        /// </summary>
        private void Update()
        {
            SpeedControl();
            GroundCheck();
            StateHandler();
            UpdateAnimation();
        }
        
        /// <summary>
        /// Called every fixed framerate frame.
        /// Calls the methods that are needed to update the player.
        /// The Methods Called use Physics.
        /// </summary>
        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateJump();
            UpdateCrouch();
        }
        
        /// <summary>
        /// This method controls the movement of the player.<br/>
        /// It gets the input values from the controls.<br/>
        /// And calculates the direction the player is moving in.<br/>
        /// Then adds a force to the player in that direction. If the player is on a slope it adds a force in the direction of the slope.<br/>
        /// It also checks if the player is on a slope and if so it adds a force to the player in the direction of the slope.<br/>
        /// The Player also stops sliding when they are standing on a slope.
        /// </summary>
        private void UpdateMovement()
        {
            _moveDirection = _orientation.forward * _input.y + _orientation.right * _input.x;
            _rigidbody.AddForce(!isJumping? _moveDirection * (currentSpeed * speedMultiplier * 2) : isGrounded ? _moveDirection.normalized * (currentSpeed * speedMultiplier) : _moveDirection.normalized * (currentSpeed * speedMultiplier * airSpeedMultiplier), ForceMode.Force);
        }

        /// <summary>
        /// It controls the animation of the player.
        /// When player is moving it sets the moveX and moveY parameters of the animator to the input values.
        /// </summary>
        private void UpdateAnimation()
        {
            //animator.SetFloat(MoveX, Mathf.Lerp(animator.GetFloat(MoveX), _input.x, Time.deltaTime * 10));
            //animator.SetFloat(MoveY, Mathf.Lerp(animator.GetFloat(MoveY), _input.y, Time.deltaTime * 10));
        }
        
        /// <summary>
        /// This method updates the jumping behavior for a player character in a game.<br/>
        /// When the player is on the ground and initiates a jump, the character jumps by setting its vertical velocity to 0 and adding an upward force.<br/>
        /// The method also keeps track of whether the player is currently jumping using a boolean <c>"isJumping"</c>.<br/>
        /// To prevent the player from double jumping, a <c>"_startJump"</c> boolean is used to track if the player has started jumping.<br/>
        /// The method also includes a timer <c>"_jumpGroundCheckTimer"</c> to wait a set time after jumping before resetting the isJumping boolean.
        /// It is used because in the first few frames after jumping, the player is still technically on the ground.
        /// </summary>
        private void UpdateJump()
        {
            var velocity = _rigidbody.velocity;
            if (isGrounded && _jumpInput && !_startJump)
            {
                isJumping = true;
                _startJump = true;
                velocity.y = 0;
                _rigidbody.velocity = velocity;
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _jumpGroundCheckTimer = JumpGroundCheckDelay;
            }

            if (isGrounded)
            {
                if(_jumpGroundCheckTimer <= 0)
                    isJumping = false;
                if (_jumpGroundCheckTimer <= 0 && (holdToJump || !_jumpInput))
                   _startJump = false;
            }
            else
                _jumpGroundCheckTimer -= Time.deltaTime;
        }
        
        
        /// <summary>
        /// This method controls the crouching behavior for a player character in a game.<br/>
        /// It sets the IsCrouching parameter of the animator to the value of the _isCrouching variable.
        /// Then it lerps the height of the collider to the crouch height or the current height depending on the value of _isCrouching.
        /// </summary>
        private void UpdateCrouch()
        {
            //animator.SetBool(IsCrouching, _isCrouching);
            var tempHeight = _isCrouching ? crouchHeight : _currentHeight;
            _collider.height = Mathf.Lerp(_collider.height, tempHeight, Time.deltaTime * 50);
        }

        /// <summary>
        /// The "SpeedControl" method is used to control the speed of the Player with a <c>"Rigidbody"</c> component.<br/>
        /// It checks the magnitude of the object's velocity and limits it to a certain value, <c>"currentSpeed"</c>, if it exceeds that value.<br/>
        /// The method takes into account if the object is on a slope or not and adjusts the velocity accordingly.<br/>
        /// The purpose of this method is to keep the object's speed within certain bounds for smoother and more controlled movement.
        /// </summary>
        private void SpeedControl()
        {
            var velocity = _rigidbody.velocity;
            var flatVelocity = velocity;
            flatVelocity.y = 0;

            if (flatVelocity.magnitude > currentSpeed)
            {
                flatVelocity = flatVelocity.normalized * currentSpeed;
                velocity = flatVelocity;
                velocity.y = _rigidbody.velocity.y;
            }
            _rigidbody.velocity = velocity;
        }
        
        /// <summary>
        /// This Method checks if the Player is touching the ground by using a raycast.<br/>
        /// The raycast is shot downwards from the center of the Player and the length of the raycast is set to a quarter of the Players's height.<br/>
        /// If the raycast hits the ground, the Method sets the Player's drag value to a specified number.<br/>
        /// If the raycast does not hit the ground, the drag value is set to 0.
        /// </summary>
        private void GroundCheck()
        {
            var transform1 = transform;
            isGrounded = Physics.Raycast(transform1.position, Vector3.down, transform1.localScale.y / 4);
            _rigidbody.drag = isGrounded ? drag : 0;
        }


        /// <summary>
        /// This Method determines the movement speed of the Player based on its current state (climbing, sliding, crouching, sprinting, walking, or in the air).
        /// The Method sets the desired speed of the Player based on its state and, if the desired speed changes drastically from the previous frame, smoothly changes the Player's current speed to the new desired speed.
        /// The current speed of the Player is continuously updated to match the desired speed.
        /// </summary>
        private void StateHandler()
        {
            // Crouching
            if (_isCrouching)
                _desiredSpeed = crouchSpeed;
            // Sprinting
            else if(isGrounded && _isRunning)
                _desiredSpeed = runningSpeed;
            // Walking
            else if (isGrounded)
                _desiredSpeed = walkingSpeed;
            // Air
            else
            {
            }
            
            if(Mathf.Abs(_desiredSpeed - _lastDesiredSpeed) > 4f && currentSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
                currentSpeed = _desiredSpeed;
            
            _lastDesiredSpeed = _desiredSpeed;
        }
        
        /// <summary>
        /// This Coroutine smoothly changes the Player's current speed to the new desired speed.
        /// The Coroutine is called in the <c>StateHandler</c> method.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // smoothly lerp movementSpeed to desired value
            float time = 0;
            var difference = Mathf.Abs(_desiredSpeed - currentSpeed);
            var startValue = currentSpeed;
            while (time < difference)
            {
                currentSpeed = Mathf.Lerp(startValue, _desiredSpeed, time / difference);
                time += Time.deltaTime * speedMultiplier;
                yield return null;
            }
            currentSpeed = _desiredSpeed;
        }
    }
}
