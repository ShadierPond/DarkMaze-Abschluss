using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveController : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        public void MoveTo(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}