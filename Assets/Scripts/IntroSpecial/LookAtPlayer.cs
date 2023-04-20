using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float damping = 6.0f;
    
    /// <summary>
    /// This method is called once per frame. It rotates the game object to face the player smoothly, ignoring the y-axis.
    /// </summary>
    private void Update()
    {
        // Calculate the direction vector from this game object to the player, ignoring the y-axis
        var lookPos = player.position - transform.position;
        lookPos.y = 0;
        // Calculate the rotation quaternion that points to the direction vector
        var rotation = Quaternion.LookRotation(lookPos);
        // Interpolate between the current rotation and the target rotation with a smoothing factor
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
    
}
