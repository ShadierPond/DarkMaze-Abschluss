using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveController : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;

        /// <summary>
        /// This method is called when the script instance is being loaded.
        /// It requires that the game object has a NavMeshAgent and an Animator component attached.
        /// </summary>
        /// <remarks>
        /// It assigns the NavMeshAgent and Animator components to the agent and animator fields respectively.
        /// </remarks>
        private void OnEnable()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// This method moves the game object to a given position on the NavMesh.
        /// </summary>
        /// <param name="position">Vector3 - The target position to move to.</param>
        /// <remarks>
        /// It samples the NavMesh around the target position to find a valid position within 4 units of distance and any area type.
        /// It sets the agent's destination to the sampled position.
        /// </remarks>
        public void MoveTo(Vector3 position)
        {
            NavMesh.SamplePosition(position, out var hit, 4f, NavMesh.AllAreas);
            position = hit.position;
            agent.SetDestination(position);
        }

        /// <summary>
        /// This method destroys the game object.
        /// </summary>
        public void SelfDestruct()
            => Destroy(gameObject);
    }
}