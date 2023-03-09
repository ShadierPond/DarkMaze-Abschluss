using UnityEngine;

namespace Weapon
{
    [DisallowMultipleComponent]
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponSelector gunSelector;
        private DefaultControls _controls;

        private void Awake()
        {
            _controls = new DefaultControls();
            _controls.Player.Shoot.performed += _ => gunSelector.activeGun.Shoot();
        }
    }
}