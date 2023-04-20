using System;
using Management;
using UnityEngine;

namespace MazeSystem
{
    public class ExitCell : MonoBehaviour
    {
        private enum WhereToGo
        {
            Hub,
            RandomLevel,
            Level1,
            Level2,
            Level3,
            Credits
        }
        [SerializeField] private WhereToGo whereToGo;
        [SerializeField] private Animator animator;
        private static readonly int PlayerIn = Animator.StringToHash("PlayerIn");
        private Transform _playerTransform;

        /// <summary>
        /// This method is called once per frame for every collider that is touching the trigger. It checks if the collider belongs to the player and sets the animator boolean parameter to true. It also assigns the player transform to a private field.
        /// </summary>
        /// <param name="other">Collider - The collider that is touching the trigger.</param>
        private void OnTriggerStay(Collider other)
        {
            // Check if the collider does not have the Player tag
            if (!other.CompareTag("Player")) 
                // If yes, return from the method
                return;
            // If not, set the animator boolean parameter to true
            animator.SetBool(PlayerIn, true);
            // Assign the collider transform to the player transform field
            _playerTransform = other.transform;
        }

        /// <summary>
        /// This method is called when a collider exits the trigger. It checks if the collider belongs to the player and sets the animator boolean parameter to false. It also sets the player transform to null.
        /// </summary>
        /// <param name="other">Collider - The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            // Check if the collider does not have the Player tag
            if (!other.CompareTag("Player")) 
                // If yes, return from the method
                return;
            // If not, set the animator boolean parameter to false
            animator.SetBool(PlayerIn, false);
            // Set the player transform to null
            _playerTransform = null;
        }

        /// <summary>
        /// This method loads a scene based on the whereToGo enum value. It requires that the player transform is not null.
        /// </summary>
        public void Exit()
        {
            // Check if the player transform is null
            if (_playerTransform == null) 
                // If yes, return from the method
                return;
            // If not, switch on the whereToGo enum value
            switch (whereToGo)
            {
                // If it is Hub, load the Hub scene using the GameManager instance
                case WhereToGo.Hub:
                    GameManager.Instance.LoadScene("Hub");
                    break;
                // If it is RandomLevel, load the Game scene using the GameManager instance
                case WhereToGo.RandomLevel:
                    GameManager.Instance.LoadScene("Game");
                    break;
                // If it is Level1, load the Level1 scene using the GameManager instance
                case WhereToGo.Level1:
                    GameManager.Instance.LoadScene("Level1");
                    break;
                // If it is Level2, load the Level2 scene using the GameManager instance
                case WhereToGo.Level2:
                    GameManager.Instance.LoadScene("Level2");
                    break;
                // If it is Level3, load the Level3 scene using the GameManager instance
                case WhereToGo.Level3:
                    GameManager.Instance.LoadScene("Level3");
                    break;
                // If it is Credits, load the Credits scene using the GameManager instance
                case WhereToGo.Credits:
                    GameManager.Instance.LoadScene("Credits");
                    break;
                // If it is none of the above cases, throw an exception
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
