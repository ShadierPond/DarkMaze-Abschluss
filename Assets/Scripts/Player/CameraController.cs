using UnityEngine;
using Cinemachine;

namespace Player
{
    
    public class CameraController : MonoBehaviour
    {
        [Header("General Settings")] 
        [SerializeField] private bool invertYAxis;
        [SerializeField] private float mouseSensitivity = 1;
        [SerializeField] private float maxLookAngle = 90;
        [SerializeField] private float minLookAngle = -90;

        [Header("First Person Settings")]
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
        [SerializeField, Range(60, 120)] private float firstPersonFOV = 90;

        private DefaultControls _controls;
        private Vector2 _axis;
        private Vector2 _rotation;
        [SerializeField] private Transform followTarget;

        /// <summary>
        /// Sets the Input System controls. It also sets the callbacks for the Look and Move actions.
        /// </summary>
        private void OnEnable()
        {
            _controls = new DefaultControls();
            _controls.Enable();
            _controls.Player.Look.performed += ctx => _axis = ctx.ReadValue<Vector2>();
            _controls.Player.Look.canceled += _ => _axis = Vector2.zero;
            
            _rotation = transform.localEulerAngles;
        }
        
        /// <summary>
        /// Disables the Input System controls when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            _controls.Disable();
        }

        /// <summary>
        /// Sets the follow target to the child of the Player object.
        /// It also calls the OnValidate method on startup.
        /// </summary>
        private void Awake()
        {
            OnValidate();
            CursorLock(true);
        }
        
        /// <summary>
        /// Changes the camera to the selected camera type.
        /// </summary>
        private void OnValidate()
        {
            if(firstPersonCamera)
                firstPersonCamera.m_Lens.FieldOfView = firstPersonFOV;
        }

        /// <summary>
        /// Updates the camera rotation.
        /// If the camera type is not first person or third person, it will reset the rotation to 0 to prevent the player from being rotated.
        /// It also stops the Rotation if the camera type is not first person or third person.
        /// </summary>
        private void Update()
        {
            CameraRotation();
        }

        /// <summary>
        /// Rotates the follow target. The follow target is the child of the player object.
        /// Cinemachine is configured to follow and look at the follow target.
        /// If the follow target is rotated, the camera will rotate with it.
        /// If the camera type is first person, the follow target will not rotate.
        /// </summary>
        private void CameraRotation()
        {
            if(invertYAxis)
                _axis.y *= -1;
            
            _rotation.x -= _axis.y * mouseSensitivity * Time.deltaTime;
            _rotation.y += _axis.x * mouseSensitivity * Time.deltaTime;
            _rotation.x = Mathf.Clamp(_rotation.x, minLookAngle, maxLookAngle);
            
            transform.rotation = Quaternion.Euler(0, _rotation.y, 0);
            followTarget.rotation = Quaternion.Euler(_rotation.x, followTarget.rotation.eulerAngles.y, 0);
        }

        /// <summary>
        /// Locks and hides the cursor if the parameter is true.
        /// Unlocks and shows the cursor if the parameter is false.
        /// </summary>
        /// <param name="lockCursor">bool - state of the cursor</param>
        private static void CursorLock(bool lockCursor)
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
        
    }
}
