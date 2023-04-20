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
        private static readonly int Shoot = Animator.StringToHash("Shoot");


        /// <summary>
        /// A method that is called when the game object is enabled.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a DefaultControls component and a playerAnimator component attached.
        /// This method creates a new instance of the DefaultControls component and assigns it to the _controls field. It also enables the _controls component and subscribes to its events for moving, sprinting and shooting. It uses lambda expressions to define the event handlers for each event. It updates the _input field with the value of the Move event, the _isRunning field with the value of the Sprint event, and triggers the Shoot animation of the playerAnimator component with the Shoot event. It also checks if the holdToRun field is true and if so, it unsubscribes from the Sprint event when it is canceled and sets the _isRunning field to false.
        /// </remarks>
        private void OnEnable()
        {
            // Create a new instance of the DefaultControls component and assign it to the _controls field.
            _controls = new DefaultControls();
            // Enable the _controls component.
            _controls.Enable();
            // Subscribe to the Move event of the Player action map of the _controls component and update the _input field with the value of the event using a lambda expression.
            _controls.Player.Move.performed += ctx => _input = ctx.ReadValue<Vector2>();
            // Subscribe to the Move cancel event of the Player action map of the _controls component and set the _input field to zero using a lambda expression.
            _controls.Player.Move.canceled += _ => _input = Vector2.zero;
            // Subscribe to the Sprint event of the Player action map of the _controls component and update the _isRunning field with the value of the event using a lambda expression. If holdToRun is false, toggle the _isRunning field with each button press. If holdToRun is true, set the _isRunning field to true or false depending on whether the button is pressed or released.
            _controls.Player.Sprint.performed += ctx => _isRunning = holdToRun ? ctx.ReadValueAsButton() : ctx.ReadValueAsButton() ? !_isRunning : _isRunning;
            // If holdToRun is true, subscribe to the Sprint cancel event of the Player action map of the _controls component and set the _isRunning field to false using a lambda expression.
            if(holdToRun)
                _controls.Player.Sprint.canceled += _ => _isRunning = false;
            // Subscribe to the Shoot event of the Player action map of the _controls component and trigger the Shoot animation of the playerAnimator component using a lambda expression.
            _controls.Player.Shoot.performed += _ => playerAnimator.SetTrigger(Shoot);
        }
        
        /// <summary>
        /// Disables the controls when the game object is disabled.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a _controls field of type InputActionAsset.
        /// </remarks>
        private void OnDisable()
            => _controls.Disable();

        /// <summary>
        /// Initializes the _rigidbody and gunSelector fields when the game object is awake.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a Rigidbody component and a PlayerWeaponSelector component.
        /// </remarks>
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Updates the game object's state every frame.
        /// </summary>
        private void Update()
        {
            SpeedControl();
            GroundCheck();
            StateHandler();
            UpdateAnimation();
    
            // Calculate the move direction based on the input and the transform
            var transform1 = transform;
            _moveDirection = transform1.forward * _input.y + transform1.right * _input.x;
        }

        /// <summary>
        /// Updates the game object's movement every fixed frame.
        /// </summary>
        private void FixedUpdate()
            => UpdateMovement();

        /// <summary>
        /// Applies a force to the rigidbody based on the move direction and speed.
        /// </summary>
        private void UpdateMovement()
            => _rigidbody.AddForce(_moveDirection.normalized * (currentSpeed * speedMultiplier), ForceMode.Force);
        
        /// <summary>
        /// Updates the animation parameters based on the input values.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a playerAnimator field of type Animator and an _input field of type Vector2.
        /// </remarks>
        private void UpdateAnimation()
        {
            // Set the MoveX and MoveY parameters of the animator using a linear interpolation of the input values
            playerAnimator.SetFloat(MoveX, Mathf.Lerp(playerAnimator.GetFloat(MoveX), _input.x, Time.deltaTime * 10));
            playerAnimator.SetFloat(MoveY, Mathf.Lerp(playerAnimator.GetFloat(MoveY), _input.y, Time.deltaTime * 10));
        }

        /// <summary>
        /// Controls the speed of the game object based on its rigidbody velocity.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a _rigidbody field of type Rigidbody, a currentSpeed field of type float and a speedMultiplier field of type float.
        /// </remarks>
        private void SpeedControl()
        {
            // Get the velocity of the rigidbody
            var velocity = _rigidbody.velocity;
            // Create a copy of the velocity with zero y component
            var flatVelocity = velocity;
            flatVelocity.y = 0;

            // If the magnitude of the flat velocity is greater than the current speed
            if (flatVelocity.magnitude > currentSpeed)
            {
                // Normalize the flat velocity and multiply it by the current speed
                flatVelocity = flatVelocity.normalized * currentSpeed;
                // Set the velocity to the modified flat velocity with the original y component
                velocity = flatVelocity;
                velocity.y = _rigidbody.velocity.y;
            }
            // Set the rigidbody velocity to the modified velocity
            _rigidbody.velocity = velocity;
        }
        
        /// <summary>
        /// Checks if the game object is grounded by casting a ray downwards.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has an isGrounded field of type bool, a _rigidbody field of type Rigidbody and a drag field of type float.
        /// </remarks>
        private void GroundCheck()
        {
            // Cast a ray from the position of the game object downwards
            var transform1 = transform;
            isGrounded = Physics.Raycast(transform1.position, Vector3.down, transform1.localScale.y / 4);
            // Set the drag of the rigidbody depending on whether the game object is grounded or not
            _rigidbody.drag = isGrounded ? drag : 0;
        }

        /// <summary>
        /// Handles the state of the game object based on its grounded status and input values.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has an isGrounded field of type bool, an _isRunning field of type bool, a _desiredSpeed field of type float, a runningSpeed field of type float, a walkingSpeed field of type float, a _lastDesiredSpeed field of type float, a currentSpeed field of type float and a speedMultiplier field of type float.
        /// </remarks>
        private void StateHandler()
        {
            _desiredSpeed = isGrounded switch
            {
                // Sprinting
                true when _isRunning => runningSpeed,
                // Walking
                true => walkingSpeed,
                _ => _desiredSpeed
            };
            
            // If the difference between the desired speed and the last desired speed is large and the current speed is not zero
            if(Mathf.Abs(_desiredSpeed - _lastDesiredSpeed) > 4f && currentSpeed != 0)
            {
                // Stop all coroutines and start a new one to smoothly lerp the current speed to the desired speed
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
                // Otherwise, set the current speed to the desired speed
                currentSpeed = _desiredSpeed;
            
            // Set the last desired speed to the desired speed
            _lastDesiredSpeed = _desiredSpeed;
        }

        /// <summary>
        /// Smoothly lerps the current speed to the desired speed over time.
        /// </summary>
        /// <returns>A IEnumerator that can be used with StartCoroutine.</returns>
        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // Initialize a time variable
            float time = 0;
            // Calculate the difference between the desired speed and the current speed
            var difference = Mathf.Abs(_desiredSpeed - currentSpeed);
            // Store the current speed as a start value
            var startValue = currentSpeed;
            // While the time is less than the difference
            while (time < difference)
            {
                // Lerp the current speed from the start value to the desired value based on the time and difference ratio
                currentSpeed = Mathf.Lerp(startValue, _desiredSpeed, time / difference);
                // Increment the time by delta time multiplied by speed multiplier
                time += Time.deltaTime * speedMultiplier;
                // Yield until next frame
                yield return null;
            }
            // Set the current speed to the desired speed
            currentSpeed = _desiredSpeed;
        }
    }
}
