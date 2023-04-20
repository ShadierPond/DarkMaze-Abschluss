using System.Collections;
using NPC;
using UnityEngine;
using UnityEngine.Pool;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        public WeaponType type;
        public ShootConfiguration shootConfiguration;
        public TrailConfiguration trailConfiguration;
        [SerializeField] private ParticleSystem shootSystem;
        private ObjectPool<TrailRenderer> _trailPool;
        [SerializeField] private Transform trailPoolParent;
        
        public void Start()
        {
            _trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        }

        public void Shoot()
        {
            shootSystem.Play();

            var shootDirection = shootSystem.transform.forward;
            shootDirection.Normalize();

            var position = shootSystem.transform.position;
            StartCoroutine(
                Physics.Raycast(position, shootDirection, out var hit, float.MaxValue, shootConfiguration.hitMask)
                    ? PlayTrail(position, hit.point, hit)
                    : PlayTrail(position,
                        position + shootDirection * trailConfiguration.missDistance, new RaycastHit())
            );

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
            trail.transform.position = start;
            trail.gameObject.SetActive(true);
            yield return null; // Wait for the trail to be initialized
            
            trail.emitting = true;
            
            var distance = Vector3.Distance(start, end);
            var remainingDistance = distance;
            while (remainingDistance > 0)
            {
                trail.transform.position = Vector3.Lerp(start, end, 1 - remainingDistance / distance);
                remainingDistance -= trailConfiguration.simulationSpeed * Time.deltaTime;
                yield return null;
            }
            trail.transform.position = end;


            if (hit.collider == null)
            {
                trail.emitting = false;
                trail.gameObject.SetActive(false);
                _trailPool.Release(trail);
                yield break;
            }
            if(hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<NonPlayerCharacter>()?.TakeDamage(shootConfiguration.damage);
                hit.collider.GetComponent<DummyAI>()?.Die();
            }
            
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
            trail.emitting = false;
            trail.transform.SetParent(trailPoolParent, false);
            trail.colorGradient = trailConfiguration.color;
            trail.material = trailConfiguration.material;
            trail.widthCurve = trailConfiguration.widthCurve;
            trail.time = trailConfiguration.duration;
            trail.minVertexDistance = trailConfiguration.minVertexDistance;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            return trail;
        }
    }
}