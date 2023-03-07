using System;
using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI
{
    [System.Serializable]
    public abstract class Consideration : ScriptableObject
    {
        private float _score;
        public float score
        {
            get { return _score; }
            set { _score = Mathf.Clamp01(value); }
        }

        public virtual void Awake()
        {
            score = 0;
        }

        public abstract float ScoreConsideration(NonPlayerCharacter npc);
    }
}