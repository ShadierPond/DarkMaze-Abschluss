using UnityEngine;

namespace Weapon
{
    [DisallowMultipleComponent]
    public class PlayerWeaponSelector : MonoBehaviour
    {
        [Space]
        public Weapon currentWeapon;
        /// <summary>
        /// Shoots the current weapon of the game object.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a currentWeapon field of type Weapon and that the Weapon class has a Shoot method.
        /// </remarks>
        public void ShootCurrentWeapon()
            // Call the Shoot method of the current weapon
            => currentWeapon.Shoot();
        
    }
}