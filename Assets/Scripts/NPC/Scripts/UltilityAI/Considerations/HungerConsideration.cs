using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "HungerConsideration", menuName = "UtilityAI/Considerations/Hunger")]
    public class HungerConsideration : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            score = responseCurve.Evaluate(npc.stats.hunger / 100f);
            return 0.2f;
        }
    }
}