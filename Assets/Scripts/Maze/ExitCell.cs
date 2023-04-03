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
        [SerializeField] private float coolDown = 1f;
        private Coroutine exitMazeCoroutine;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (exitMazeCoroutine != null)
                    StopCoroutine(exitMazeCoroutine);
                exitMazeCoroutine = StartCoroutine(ExitMaze());
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (exitMazeCoroutine != null)
                    StopCoroutine(exitMazeCoroutine);
            }
        }
        
        private IEnumerator ExitMaze()
        {
            var time = 0f;
            while (time < coolDown)
            {
                time += Time.deltaTime;
                
                yield return null;
            }
            GameManager.Instance.OnMazeExit();
        }
    }
}
