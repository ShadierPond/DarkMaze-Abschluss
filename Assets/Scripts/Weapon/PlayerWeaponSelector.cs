using System;
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

        [Space]
        public Weapon activeGun;

        private void Start()
        {
            var newGun = weapons.Find(g => g.type == activeWeapon);

            activeGun = newGun;
        }
    }
}