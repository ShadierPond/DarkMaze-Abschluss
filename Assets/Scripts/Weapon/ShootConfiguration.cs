using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapons/Shoot Configuration", order = 2)]
    public class ShootConfiguration : ScriptableObject
    {
        public LayerMask hitMask;
        public float damage = 10f;
    }
}