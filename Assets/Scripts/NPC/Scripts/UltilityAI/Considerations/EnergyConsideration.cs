using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "EnergyConsideration", menuName = "UtilityAI/Considerations/Energy")]
    public class EnergyConsideration : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            return 0.1f;
        }
    }
}