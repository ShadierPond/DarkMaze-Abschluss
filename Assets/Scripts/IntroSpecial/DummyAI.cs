using System.Collections;
using NPC;
using UnityEngine;

public class DummyAI : MonoBehaviour
{
    private Animator _animator;
    private MoveController _moveController;
    [SerializeField] private float moveDistance = 5f;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    [SerializeField] private float waitTime = 1f;
    private static readonly int Walking = Animator.StringToHash("IsWalking");
    private static readonly int Die1 = Animator.StringToHash("Die");

    /// <summary>
    /// This method is called when the game object is enabled. It moves the game object from its current position to a point along its forward direction with a specified distance.
    /// </summary>
    /// <remarks>
    /// It requires that the game object has a MoveController component and an Animator component attached. It also requires that the game object is on a NavMesh surface.
    /// </remarks>
    private void OnEnable()
    {
        // Get the MoveController component of the game object
        _moveController = GetComponent<MoveController>();
        // Get the Animator component of the game object
        _animator = GetComponent<Animator>();
        // Get the current transform of the game object
        var transform1 = transform;
        // Set the start point to the current position
        _startPoint = transform1.position;
        // Set the end point to a point along the forward direction with a specified distance
        _endPoint = _startPoint + transform1.forward * moveDistance;
        // Call the MoveTo method of the moveController component to move to the end point
        while (_moveController.agent == null)
            return;
        if(!_moveController.agent.isOnNavMesh)
            return;
        _moveController.MoveTo(_endPoint);
    }

    /// <summary>
    /// This method is called when the script is loaded or a value is changed in the inspector. It sets the start point and the end point of the game object based on its current position and forward direction.
    /// </summary>
    private void OnValidate()
    {
        // Get the current transform of the game object
        var transform1 = transform;
        // Set the start point to the current position
        _startPoint = transform1.position;
        // Set the end point to a point along the forward direction with a specified distance
        _endPoint = _startPoint + transform1.forward * moveDistance;
    }

    /// <summary>
    /// This method is called once per frame. It updates the animation state of the game object based on the remaining distance to the destination.
    /// </summary>
    private void Update()
    {
        // If the game object is not on a NavMesh surface, return
        if(!_moveController.agent.isOnNavMesh)
            return;
        // If the remaining distance is less than or equal to 0.1f, stop walking and start waiting
        if (_moveController.agent.remainingDistance <= 0.1f)
        {
            _animator.SetBool(Walking, false);
            StartCoroutine(WaitAndMove());
        }

        // If the remaining distance is greater than 0.1f, keep walking
        if (_moveController.agent.remainingDistance > 0.1f)
            _animator.SetBool(Walking, true);
    }
    
    /// <summary>
    /// This method is a coroutine that waits for a specified time and then moves the game object to the opposite point.
    /// </summary>
    /// <returns>
    /// IEnumerator - a sequence of instructions that can be paused and resumed.
    /// </returns>
    private IEnumerator WaitAndMove()
    {
        // Swap the start and end points using tuple assignment
        (_startPoint, _endPoint) = (_endPoint, _startPoint);
        // Wait for the waitTime seconds
        yield return new WaitForSeconds(waitTime);
        // Move to the new end point
        _moveController.MoveTo(_endPoint);
    }

    /// <summary>
    /// This method is called when the game object dies. It sets the Die trigger of the animator component to play the death animation.
    /// </summary>
    public void Die()
    {
        _animator.SetTrigger(Die1);
    }
    
    /// <summary>
    /// This method draws gizmos in the scene view to visualize the start and end points of the movement and the direction of the movement.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Set the gizmo color to red and draw a sphere at the start point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_startPoint, 0.5f);
        // Draw a line from the start point to the end point
        Gizmos.DrawLine(_startPoint, _endPoint);
        // Set the gizmo color to green and draw a sphere at the end point
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_endPoint, 0.5f);
    }
}
