using System.Collections;
using NPC;
using UnityEngine;

public class DummyAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MoveController moveController;
    [SerializeField] private float moveDistance = 5f;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    [SerializeField] private float waitTime = 1f;
    private static readonly int Walking = Animator.StringToHash("IsWalking");

    /// <summary>
    /// This method sets the start and end points for the movement of the game object and initiates the movement using the moveController component.
    /// </summary>
    /// <remarks>
    /// It requires that the game object has a moveController component attached to it.
    /// </remarks>
    private void Awake()
    {
        // Get the current transform of the game object
        var transform1 = transform;
        // Set the start point to the current position
        _startPoint = transform1.position;
        // Set the end point to a point along the forward direction with a specified distance
        _endPoint = _startPoint + transform1.forward * moveDistance;
        // Call the MoveTo method of the moveController component to move to the end point
        moveController.MoveTo(_endPoint);
    }

    /// <summary>
    /// This method checks the remaining distance to the destination and sets the walking animation state accordingly. It also calls the WaitAndMove coroutine when the destination is reached.
    /// </summary>
    private void Update()
    {
        // If the remaining distance is less than or equal to 0.1f, stop walking and start waiting
        if (moveController.agent.remainingDistance <= 0.1f)
        {
            animator.SetBool(Walking, false);
            StartCoroutine(WaitAndMove());
        }

        // If the remaining distance is greater than 0.1f, keep walking
        if (moveController.agent.remainingDistance > 0.1f)
        {
            animator.SetBool(Walking, true);
        }
    }
    
    /// <summary>
    /// This coroutine swaps the start and end points and waits for a specified time before moving to the new end point.
    /// </summary>
    /// <returns>
    /// IEnumerator - a sequence of actions that can be paused and resumed.
    /// </returns>
    private IEnumerator WaitAndMove()
    {
        // Swap the start and end points using tuple assignment
        (_startPoint, _endPoint) = (_endPoint, _startPoint);
        // Wait for the waitTime seconds
        yield return new WaitForSeconds(waitTime);
        // Move to the new end point
        moveController.MoveTo(_endPoint);
    }
    
    /// <summary>
    /// This method draws gizmos in the scene view to visualize the start and end points of the movement and the direction of the movement.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Get the current transform of the game object
        var transform1 = transform;
        // Calculate the start and end points based on the current position and forward direction
        var start = transform1.position;
        var end = start + transform1.forward * moveDistance;
        // Set the gizmo color to red and draw a sphere at the start point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(start, 0.5f);
        // Draw a line from the start point to the end point
        Gizmos.DrawLine(start, end);
        // Set the gizmo color to green and draw a sphere at the end point
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(end, 0.5f);
    }
}
