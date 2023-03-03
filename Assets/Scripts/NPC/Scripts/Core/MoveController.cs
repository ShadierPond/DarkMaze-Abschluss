using UnityEngine;
using UnityEngine.AI;

namespace NPC.Core
{
    public class MoveController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        public void MoveTo(Vector3 position)
        {
            _agent.SetDestination(position);
        }
    }
}