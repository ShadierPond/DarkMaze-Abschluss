using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Weapon
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Gun : ScriptableObject
    {
        public WeaponType type;
        public new string name;
        public AssetReferenceGameObject modelPrefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        
        public ShootConfiguration shootConfiguration;
        public TrailConfiguration trailConfiguration;
        
        private MonoBehaviour _activeMonoBehaviour;
        private GameObject _model;
        private float _lastShootTime;
        private ParticleSystem _shootSystem;
        private ObjectPool<TrailRenderer> _trailPool;

        /// <summary>
        /// Spawns the gun model and initializes the shoot system
        /// Creates a pool of trails
        /// </summary>
        /// <param name="parent">Transform - The parent of the gun model </param>
        /// <param name="activeMonoBehaviour">MonoBehaviour - The active MonoBehaviour </param>
        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            _activeMonoBehaviour = activeMonoBehaviour;
            _lastShootTime = 0;
            _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
            //_model = Instantiate(modelPrefab);
            _model.transform.SetParent(parent, false);
            _model.transform.localPosition = spawnPosition;
            _model.transform.localRotation = Quaternion.Euler(spawnRotation);
            
            _shootSystem = _model.GetComponentInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            if(Time.time > shootConfiguration.fireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;
                _shootSystem.Play();
                Vector3 shootDirection = _shootSystem.transform.forward + new Vector3(
                    Random.Range(-shootConfiguration.spread.x, shootConfiguration.spread.x), 
                    Random.Range(-shootConfiguration.spread.y, shootConfiguration.spread.y), 
                    Random.Range(-shootConfiguration.spread.z, shootConfiguration.spread.z)
                    );
                shootDirection.Normalize();
                
                if(Physics.Raycast(_shootSystem.transform.position, shootDirection, out var hit, float.MaxValue, shootConfiguration.hitMask))
                {
                    _activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position, hit.point, hit));
                }
                else
                {
                    _activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position, _shootSystem.transform.position + shootDirection * trailConfiguration.missDistance, new RaycastHit()));
                }
            }
        }
        
        /// <summary>
        /// Plays a trail from start to end
        /// </summary>
        /// <param name="start"> Vector3 - The start position of the trail </param>
        /// <param name="end"> Vector3 - The end position of the trail </param>
        /// <param name="hit"> RaycastHit - The hit information </param>
        /// <returns> IEnumerator - The coroutine </returns>
        private IEnumerator PlayTrail(Vector3 start, Vector3 end, RaycastHit hit)
        {
            var trail = _trailPool.Get();
            trail.gameObject.SetActive(true);
            trail.transform.position = start;
            yield return null; // Wait for the trail to be initialized
            
            trail.emitting = true;
            
            float distance = Vector3.Distance(start, end);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                trail.transform.position = Vector3.Lerp(start, end, Mathf.Clamp01(1 - remainingDistance / distance));
                remainingDistance -= trailConfiguration.simulationSpeed * Time.deltaTime;
                yield return null;
            }
            trail.transform.position = end;
            
            if(hit.collider != null)
            {
                // TODO: Handle impact when the bullet hits something
                Debug.Log("Hit " + hit.collider.name);
            }
            
            yield return new WaitForSeconds(trailConfiguration.duration);
            yield return null;
            trail.emitting = false;
            trail.gameObject.SetActive(false);
            _trailPool.Release(trail);
        }
        
        /// <summary>
        /// Creates a trail renderer and sets its properties
        /// </summary>
        /// <returns>GameObject - The trail renderer </returns>
        private TrailRenderer CreateTrail()
        {
            var trail = new GameObject("Bullet Trail").AddComponent<TrailRenderer>();
            trail.transform.SetParent(_model.transform, false);
            trail.colorGradient = trailConfiguration.color;
            trail.material = trailConfiguration.material;
            trail.widthCurve = trailConfiguration.widthCurve;
            trail.time = trailConfiguration.duration;
            trail.minVertexDistance = trailConfiguration.minVertexDistance;
            
            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            
            return trail;
        }
    }
}