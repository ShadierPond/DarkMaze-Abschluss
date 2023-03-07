using UnityEngine;
using UnityEngine.AI;

namespace NPC.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveController : MonoBehaviour
    {
        public NavMeshAgent agent;
        
        private void Start()
            => agent = GetComponent<NavMeshAgent>();

        public void MoveTo(Vector3 position)
        {
            //_agent.SetDestination(position);
            Debug.Log("Move to " + position);
        }
    }
}