using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapons/Shoot Configuration", order = 2)]
    public class ShootConfiguration : ScriptableObject
    {
        public LayerMask hitMask;
        public Vector3 spread = new Vector3(0.1f, 0.1f, 0f);
        public float fireRate = 0.25f;
    }
}