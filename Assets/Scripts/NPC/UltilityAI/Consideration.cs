using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI
{
    [System.Serializable]
    public abstract class Consideration : ScriptableObject
    {
        private float _score;
        public float Score { get => _score; set => _score = Mathf.Clamp01(value); }
        public virtual void Awake()
            => Score = 0;
        public abstract float ScoreConsideration(NonPlayerCharacter npc);
    }
}