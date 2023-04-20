using Management;
using UnityEngine;

namespace NPC
{
    public class Stats : MonoBehaviour
    {
        [Header("Player Settings")]
        [HideInInspector] public GameObject player;
        public int sightRange = 10;
        public int sightAngle = 90;

        [Header("NPC Settings")]
        public float health = 100;

        /// <summary>
        /// A method that is called when the game object is enabled.
        /// </summary>
        /// <remarks>
        /// This method assigns the player field to the game object with the "Player" tag using GameObject.FindGameObjectWithTag.
        /// </remarks>
        private void OnEnable()
            // Assign the player field to the game object with the "Player" tag.
            => player = GameObject.FindGameObjectWithTag("Player");

        /// <summary>
        /// A method that is called when the game object is disabled.
        /// </summary>
        /// <remarks>
        /// This method assigns the player field to null.
        /// </remarks>
        private void OnDisable()
            // Assign the player field to null.
            => player = null;

        /// <summary>
        /// A method that is called every frame.
        /// </summary>
        /// <remarks>
        /// This method checks if the player field is null and if so, assigns it to the game object with the "Player" tag using GameObject.FindGameObjectWithTag.
        /// </remarks>
        private void Update()
        {
            // If the player field is not null, return from the method.
            if(player != null)
                return;
            // Assign the player field to the game object with the "Player" tag.
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// A method that attacks the player by loading the "Hub" scene using GameManager.Instance.LoadScene.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a GameManager component attached and a scene named "Hub" in the project.
        /// This method checks if the player field is null and if so, returns from the method. Otherwise, it calls GameManager.Instance.LoadScene with "Hub" as the argument.
        /// </remarks>
        public void AttackPlayer()
        {
            // If the player field is null, return from the method.
            if (player == null)
                return;
            // Call GameManager.Instance.LoadScene with "Hub" as the argument.
            GameManager.Instance.LoadScene("Hub");
        }

        /// <summary>
        /// A method that draws gizmos in the scene view for debugging purposes.
        /// </summary>
        /// <remarks>
        /// This method draws a wire sphere and two lines to represent the sight range and angle of the game object using Gizmos.DrawWireSphere and Gizmos.DrawLine. It uses Color.green as the gizmo color and uses transform.position and transform.forward as reference points for drawing.
        /// </remarks>
        private void OnDrawGizmos()
        {
            // Get a reference to the transform component of the game object.
            var transform1 = transform;
            // Get a reference to the position of the transform component.
            var position = transform1.position;
            // Get a reference to the forward direction of the transform component.
            var forward = transform1.forward;
            
            // Draw Sight
            // Set the gizmo color to green.
            Gizmos.color = Color.green;
            // Draw a wire sphere at the position with a radius of sightRange.
            Gizmos.DrawWireSphere(position, sightRange);
            // Draw a line from the position to a point that is rotated by sightAngle / 2 degrees around the y-axis and multiplied by sightRange along the forward direction. 
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, sightAngle / 2f, 0) * forward * sightRange);
            // Draw a line from the position to a point that is rotated by -sightAngle / 2 degrees around the y-axis and multiplied by sightRange along the forward direction. 
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, -sightAngle / 2f, 0) * forward * sightRange);
        }
    }
}