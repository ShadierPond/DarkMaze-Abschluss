using System;
using MazeSystem;
using UnityEngine;

namespace NPC.Core
{
    public class Stats : MonoBehaviour
    {
        [Header("Player Settings")]
        public GameObject player;
        public int sightRange = 10;
        public int sightAngle = 90;

        [Header("NPC Settings")]
        public int energy = 100;
        public int health = 100;
        
        public MazeGenerator mazeGenerator;
        
        private void OnEnable()
        {
            mazeGenerator = FindObjectOfType<MazeGenerator>();
        }

        private void OnDisable()
        {
            mazeGenerator = null;
        }

        public void UpdateEnergy(int value)
            => energy += value;

        private void OnDrawGizmos()
        {

            // Draw Sight
            Gizmos.color = Color.green;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, sightRange);
            var forward = transform.forward;
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, sightAngle / 2, 0) * forward * sightRange);
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, -sightAngle / 2, 0) * forward * sightRange);
        }
    }
}