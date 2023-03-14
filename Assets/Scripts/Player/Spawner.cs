using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Player
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private AssetReference playerPrefab;
        [SerializeField] private Transform spawnPoint;
        
        private void Awake()
        {
            playerPrefab.InstantiateAsync(spawnPoint.position, spawnPoint.rotation);
        }
    }
}