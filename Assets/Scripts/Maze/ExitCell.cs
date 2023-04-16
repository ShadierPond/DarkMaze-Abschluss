using System;
using System.Collections;
using System.Collections.Generic;
using Management;
using UnityEngine;
using UnityEngine.Rendering;

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

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool(PlayerIn, true);
                _playerTransform = other.transform;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool(PlayerIn, false);
                _playerTransform = null;
            }
        }
        
        public void Exit()
        {
            if (_playerTransform != null)
            {
                
            }
        }
    }
}
