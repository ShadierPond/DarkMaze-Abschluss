using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Have Energy Left", menuName = "UtilityAI/Considerations/Have Energy Left")]
    public class HasEnergy : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
            => (float)npc.stats.energy / npc.stats.maxEnergy;
    }
}