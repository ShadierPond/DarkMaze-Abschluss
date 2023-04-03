using System;
using Management;
using UnityEngine;

namespace NPC
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

        private void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(GameManager.Instance != null)
                GameManager.Instance.RegisterEnemy(gameObject);
            else
                Debug.LogWarning("GameManager not found. Maybe it is not in the scene? Or the Enemy is being instantiated before the GameManager?");
        }

        private void OnDisable()
        { 
            player = null;
            GameManager.Instance.UnregisterEnemy(gameObject);
        }

        private void Update()
        {
            if(player != null)
                return;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void UpdateEnergy(int value)
            => energy += value;
        
        private void OnDeath()
        {
            GameManager.Instance.UnregisterEnemy(gameObject);
            Destroy(gameObject);
        }

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