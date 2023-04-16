using System.Collections;
using Management;
using UnityEngine;
using Weapon;

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
        public bool isGrounded;

        [Header("Movement Settings")]
        [SerializeField] private float walkingSpeed;
        private Vector3 _moveDirection;

        [Header("Running Settings")]
        [SerializeField] private float runningSpeed;
        [SerializeField] private bool holdToRun;
        private bool _isRunning;

        [Header("Animation Settings")]
        [SerializeField] private Animator playerAnimator;
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        
        [Header("Shoot Settings")]
        [SerializeField] private PlayerWeaponSelector gunSelector;

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
            _controls.Player.Move.canceled += _ => _input = Vector2.zero;
            _controls.Player.Sprint.performed += ctx => _isRunning = holdToRun ? ctx.ReadValueAsButton() : ctx.ReadValueAsButton() ? !_isRunning : _isRunning;
            if(holdToRun)
                _controls.Player.Sprint.canceled += _ => _isRunning = false;

            _controls.Player.Interact.performed += _ => GameManager.Instance.SaveData();
            _controls.Player.Shoot.performed += _ => gunSelector.currentWeapon.Shoot();
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
            _rigidbody = GetComponent<Rigidbody>();
            gunSelector = GetComponent<PlayerWeaponSelector>();
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
            
            _moveDirection = transform.forward * _input.y + transform.right * _input.x;
        }
        
        /// <summary>
        /// Called every fixed framerate frame.
        /// Calls the methods that are needed to update the player.
        /// The Methods Called use Physics.
        /// </summary>
        private void FixedUpdate()
        {
            UpdateMovement();
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
            _rigidbody.AddForce(_moveDirection.normalized * (currentSpeed * speedMultiplier), ForceMode.Force);
        }

        /// <summary>
        /// It controls the animation of the player.
        /// When player is moving it sets the moveX and moveY parameters of the animator to the input values.
        /// </summary>
        private void UpdateAnimation()
        {
            playerAnimator.SetFloat(MoveX, Mathf.Lerp(playerAnimator.GetFloat(MoveX), _input.x, Time.deltaTime * 10));
            playerAnimator.SetFloat(MoveY, Mathf.Lerp(playerAnimator.GetFloat(MoveY), _input.y, Time.deltaTime * 10));
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
        /// The raycast is shot downwards from the center of the Player and the length of the raycast is set to a quarter of the Player's height.<br/>
        /// If the raycast hits the ground, the Method sets the Player's drag value to a specified number.<br/>
        /// If the raycast does not hit the ground, the drag value is set to 0.
        /// </summary>
        private void GroundCheck()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y / 4);
            _rigidbody.drag = isGrounded ? drag : 0;
        }


        /// <summary>
        /// This Method determines the movement speed of the Player based on its current state (climbing, sliding, crouching, sprinting, walking, or in the air).
        /// The Method sets the desired speed of the Player based on its state and, if the desired speed changes drastically from the previous frame, smoothly changes the Player's current speed to the new desired speed.
        /// The current speed of the Player is continuously updated to match the desired speed.
        /// </summary>
        private void StateHandler()
        {
            // Sprinting
            if(isGrounded && _isRunning)
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
