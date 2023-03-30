using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    [DisallowMultipleComponent]
    public class PlayerWeaponSelector : MonoBehaviour
    {
        [SerializeField] private WeaponType activeWeapon;
        [SerializeField] private List<Weapon> weapons;
        [SerializeField] private Animator playerAnimator;

        [Space]
        public Weapon currentWeapon;
        private int _currentWeaponIndex;
        private static readonly int Type = Animator.StringToHash("WeaponType");

        private void Update()
        {
            if(_currentWeaponIndex != (int) activeWeapon)
            {
                _currentWeaponIndex = (int) activeWeapon;
                currentWeapon = weapons.Find(weapon => weapon.type == activeWeapon);
                playerAnimator.SetInteger(Type, _currentWeaponIndex);
            }
        }
    }
}