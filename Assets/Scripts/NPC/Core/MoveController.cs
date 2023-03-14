using UnityEngine;
using UnityEngine.AI;

namespace NPC.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveController : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Attack1 = Animator.StringToHash("Attack");

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        public void MoveTo(Vector3 position)
        {
            agent.SetDestination(position);
            animator.SetBool(IsWalking, true);
        }
        
        public void Attack()
        {
            animator.SetTrigger(Attack1);
        }
    }
}