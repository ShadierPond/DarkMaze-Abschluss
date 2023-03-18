using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Player
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private AssetReference playerPrefab;
        [SerializeField] private Transform spawnPoint;
        
        private void Awake()
        {
            playerPrefab.InstantiateAsync().Completed += OnPlayerSpawned;
        }
        
        private void OnPlayerSpawned(AsyncOperationHandle<GameObject> obj)
        {
            obj.Result.transform.position = spawnPoint.position;
            obj.Result.transform.GetChild(0).position = spawnPoint.position;
            obj.Result.transform.GetChild(0).rotation = spawnPoint.rotation;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position , 0.05f);
        }
    }
}