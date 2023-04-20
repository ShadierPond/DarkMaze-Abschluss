using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpawnCell : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject spawnablePrefab;
    private bool _isSpawned;

    /// <summary>
    /// This method is called when a collider enters the trigger. It checks if the spawnable object is not spawned and starts a coroutine to spawn it.
    /// </summary>
    /// <param name="other">Collider - The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the spawnable object is not spawned
        if(!_isSpawned)
            // If yes, start a coroutine to spawn it using the Spawn method
            StartCoroutine(Spawn());
    }

    /// <summary>
    /// This method is a coroutine that spawns a game object asynchronously from the spawnable prefab asset reference at the trigger position and rotation. It also sets the spawned object parent to the trigger parent and sets the spawn flag to true.
    /// </summary>
    /// <returns>IEnumerator - The coroutine enumerator.</returns>
    private IEnumerator Spawn()
    {
        // Set the spawn flag to true
        _isSpawned = true;
        // Instantiate the game object asynchronously from the spawnable prefab asset reference at the trigger position and rotation
        var handle = spawnablePrefab.InstantiateAsync(transform.position, transform.rotation);
        // Wait until the instantiation is completed
        yield return handle;
        // Get the instantiated object from the handle result
        var spawnedObject = handle.Result;
        // Set the spawned object parent to the trigger parent
        spawnedObject.transform.parent = transform.parent;
    }
}
