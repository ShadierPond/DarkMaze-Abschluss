using UnityEngine;
using UnityEngine.InputSystem;

namespace NewInput.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;

        [SerializeField] private InputActionReference 
            moveAction,
            interactAction,
            jumpAction,
            crouchAction,
            sprintAction,
            lookAction,
            scrollAction;

        private void OnEnable()
        {
            // performed is called when the button is pressed
            moveAction.action.performed += PerformMove;
            interactAction.action.performed += PerformInteract;
            jumpAction.action.performed += PerformJump;
            crouchAction.action.performed += PerformCrouch;
            sprintAction.action.performed += PerformSprint;
            lookAction.action.performed += PerformLook;
            scrollAction.action.performed += PerformScroll;
            
            // canceled is called when the button is released
            interactAction.action.canceled += CancelInteract;
            jumpAction.action.canceled += CancelJump;
            crouchAction.action.canceled += CancelCrouch;
            sprintAction.action.canceled += CancelSprint;

            // started is called when the button is held down
            interactAction.action.started += StartInteract;
            jumpAction.action.started += StartJump;
            crouchAction.action.started += StartCrouch;
            sprintAction.action.started += StartSprint;
        }
        
        private void OnDisable()
        {
            moveAction.action.performed -= PerformMove;
            interactAction.action.performed -= PerformInteract;
            jumpAction.action.performed -= PerformJump;
            crouchAction.action.performed -= PerformCrouch;
            sprintAction.action.performed -= PerformSprint;
            lookAction.action.performed -= PerformLook;
            scrollAction.action.performed -= PerformScroll;
            
            interactAction.action.canceled -= CancelInteract;
            jumpAction.action.canceled -= CancelJump;
            crouchAction.action.canceled -= CancelCrouch;
            sprintAction.action.canceled -= CancelSprint;
            
            interactAction.action.started -= StartInteract;
            jumpAction.action.started -= StartJump;
            crouchAction.action.started -= StartCrouch;
            sprintAction.action.started -= StartSprint;
        }
        
        /// <summary>
        /// This is called when the button is pressed
        /// </summary>
        /// <param name="context"></param>
        private void PerformMove(InputAction.CallbackContext context)
        {
            Debug.Log("Move - x: " + context.ReadValue<Vector2>().x + " y: " + context.ReadValue<Vector2>().y);
        }

        private void PerformInteract(InputAction.CallbackContext context)
        {
            Debug.Log("Interact - Pressed");
        }
        
        private void CancelInteract(InputAction.CallbackContext context)
        {
            Debug.Log("Interact - Released");
        }
        
        private void StartInteract(InputAction.CallbackContext context)
        {
            Debug.Log("Interact - Held");
        }
        
        private void PerformJump(InputAction.CallbackContext context)
        {
            Debug.Log("Jump - Pressed");
        }
        
        private void CancelJump(InputAction.CallbackContext context)
        {
            Debug.Log("Jump - Released");
        }
        
        private void StartJump(InputAction.CallbackContext context)
        {
            Debug.Log("Jump - Held");
        }
        
        private void PerformCrouch(InputAction.CallbackContext context)
        {
            Debug.Log("Crouch - Pressed");
        }
        
        private void CancelCrouch(InputAction.CallbackContext context)
        {
            Debug.Log("Crouch - Released");
        }
        
        private void StartCrouch(InputAction.CallbackContext context)
        {
            Debug.Log("Crouch - Held");
        }
        
        private void PerformSprint(InputAction.CallbackContext context)
        {
            Debug.Log("Sprint - Pressed");
        }
        
        private void CancelSprint(InputAction.CallbackContext context)
        {
            Debug.Log("Sprint - Released");
        }
        
        private void StartSprint(InputAction.CallbackContext context)
        {
            Debug.Log("Sprint - Held");
        }
        
        private void PerformLook(InputAction.CallbackContext context)
        {
            Debug.Log("Look");
        }
        
        private void PerformScroll(InputAction.CallbackContext context)
        {
            Debug.Log("Scroll");
        }
    }
}
