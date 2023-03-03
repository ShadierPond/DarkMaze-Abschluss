using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI
{
    public abstract class Action : ScriptableObject
    {
        public string actionName;
        private float _score;
        public float score { 
            get { return _score; }
            set { _score = Mathf.Clamp01(value); }
        }

        public Consideration[] considerations;
        
        public virtual void Awake()
        {
            score = 0;
        }

        public abstract void Execute(NonPlayerCharacter npc);
    }
}