using System;
using MazeSystem;
using NPC.UI;
using UnityEngine;

namespace NPC.Core
{
    public class Stats : MonoBehaviour
    {
        [Header("Player Settings")]
        public GameObject player;
        public int sightRange = 10;
        public int sightAngle = 90;

        [Header("Hearing Settings")]
        public int hearingRange = 10;
        public bool isHeard = false;
        public float noiseLoudness = 0;
        public float noiseRange = 0;
        public float noiseAngle = 0;
        

        [Header("NPC Settings")]
        public int energy = 100;
        public int health = 100;
        
        public MazeGenerator mazeGenerator;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnEnable()
        {
            mazeGenerator = FindObjectOfType<MazeGenerator>();
        }

        private void OnDisable()
        {
            mazeGenerator = null;
        }

        private void Update()
        {
        }
        
        public void UpdateEnergy(int value)
        {
            energy += value;
        }

        private void OnDrawGizmos()
        {

            // Draw Sight
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sightRange);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, sightAngle / 2, 0) * transform.forward * sightRange);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward * sightRange);
            
            // Draw Hearing
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hearingRange);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, noiseAngle / 2, 0) * transform.forward * noiseRange);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -noiseAngle / 2, 0) * transform.forward * noiseRange);
        }
    }
}