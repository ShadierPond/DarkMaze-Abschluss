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
        public int maxEnergy = 100;
        public int health = 100;
        
        [HideInInspector] public MazeGenerator mazeGenerator;
        
        private void OnEnable()
        {
            mazeGenerator = FindObjectOfType<MazeGenerator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnDisable()
        {
            mazeGenerator = null;
            player = null;
        }

        private void Update()
        {
            if(player != null)
                return;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void UpdateEnergy(int value)
            => energy += value;

        private void OnDrawGizmos()
        {
            var transform1 = transform;
            var position = transform1.position;
            var forward = transform1.forward;
            
            // Draw Sight
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, sightRange);
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, sightAngle / 2f, 0) * forward * sightRange);
            Gizmos.DrawLine(position, position + Quaternion.Euler(0, -sightAngle / 2f, 0) * forward * sightRange);
        }
    }
}