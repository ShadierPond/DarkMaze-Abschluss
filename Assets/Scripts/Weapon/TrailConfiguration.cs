﻿using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/Weapon Trail Config", order = 4)]
    public class TrailConfiguration : ScriptableObject
    {
        public Material material;
        public AnimationCurve widthCurve;
        public float duration = 0.5f;
        public float minVertexDistance = 0.1f;
        public Gradient color;
        
        public float missDistance = 100f;
        public float simulationSpeed = 100f;
    }
}