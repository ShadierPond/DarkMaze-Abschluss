using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "MoneyConsideration", menuName = "UtilityAI/Considerations/Money")]
    public class MoneyConsideration : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            return 0.9f;
        }
    }
}