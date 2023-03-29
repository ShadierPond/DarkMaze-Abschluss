using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor.Animations;

namespace Weapon
{
    [DisallowMultipleComponent]
    public class PlayerWeaponSelector : MonoBehaviour
    {
        [SerializeField] private WeaponType activeWeapon;
        [SerializeField] private List<Weapon> weapons;
        [SerializeField] private Transform weaponParent;
        [SerializeField] private Transform leftHandGrip;
        [SerializeField] private Transform rightHandGrip;
        [SerializeField] private Transform weaponPose;
        [SerializeField] private Transform aimPose;
        [SerializeField] private Transform leftArmHelper;
        [SerializeField] private Transform rightArmHelper;

        [Space]
        public Weapon activeGun;

        private void Start()
        {
            var newGun = weapons.Find(g => g.type == activeWeapon);

            activeGun = newGun;
        }

        [ContextMenu("Save Weapon Pose")]
        private void SaveWeaponPose()
        {
            GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
            recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
            recorder.BindComponentsOfType<Transform>(weaponPose.gameObject, false);
            recorder.BindComponentsOfType<Transform>(leftArmHelper.gameObject, false);
            recorder.BindComponentsOfType<Transform>(rightArmHelper.gameObject, false);
            recorder.BindComponentsOfType<Transform>(aimPose.gameObject, false);
            recorder.BindComponentsOfType<Transform>(leftHandGrip.gameObject, false);
            foreach (var finger in leftHandGrip.GetComponentsInChildren<Transform>())
                recorder.BindComponentsOfType<Transform>(finger.gameObject, false);
            recorder.BindComponentsOfType<Transform>(rightHandGrip.gameObject, false);
            foreach (var finger in rightHandGrip.GetComponentsInChildren<Transform>())
                recorder.BindComponentsOfType<Transform>(finger.gameObject, false);
            recorder.TakeSnapshot(0f);
            recorder.SaveToClip(activeGun.weaponAnimation);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}