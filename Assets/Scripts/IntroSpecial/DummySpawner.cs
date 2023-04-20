using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private int maxDummies = 3;
    private int _currentDummies;
    private float _spawnTime;

    /// <summary>
    /// This method is called once per frame. It spawns new dummies from a prefab at a fixed interval until the maximum number of dummies is reached.
    /// </summary>
    private void Update()
    {
        // Get the current number of dummies by counting the children of this game object
        _currentDummies = transform.childCount;

        // Increase the spawn time by the elapsed time since the last frame
        _spawnTime += Time.deltaTime;
        // If the spawn time is greater than or equal to the spawn delay and the current number of dummies is less than the maximum number of dummies
        if (_spawnTime >= spawnDelay && _currentDummies < maxDummies)
        {
            // Reset the spawn time to zero
            _spawnTime = 0f;
            // Instantiate a new dummy from the dummy prefab at this game object's position and rotation and make it a child of this game object
            var dummy = Instantiate(dummyPrefab, transform.position, transform.rotation, transform);
            // Activate the dummy
            dummy.SetActive(true);
        }
    }
}
