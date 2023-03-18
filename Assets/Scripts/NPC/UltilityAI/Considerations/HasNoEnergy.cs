using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Have No Energy Left", menuName = "UtilityAI/Considerations/Have No Energy Left")]
    public class HasNoEnergy : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
            => 1 - (float)npc.stats.energy / npc.stats.maxEnergy;
    }
}