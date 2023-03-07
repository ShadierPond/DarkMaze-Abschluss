using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "No Noise Heard Consideration", menuName = "UtilityAI/Considerations/No Noise Has Been Heard")]
    public class NoNoiseHeard : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            return npc.soundReciever.intensity <= 0.1 ? 1 : 0;
        }
    }
}