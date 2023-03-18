using UnityEngine;

namespace NPC.UtilityAI
{
    [System.Serializable]
    public abstract class Action : ScriptableObject
    {
        private float _score;
        public float Score { get => _score; set => _score = Mathf.Clamp01(value); }

        public Consideration[] considerations;
        
        public virtual void Awake()
            => Score = 0;

        public abstract void Execute(NonPlayerCharacter npc);
        
    }
}