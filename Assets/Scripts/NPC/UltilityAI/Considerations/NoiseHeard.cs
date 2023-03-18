using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Noise Heard Consideration", menuName = "UtilityAI/Considerations/Noise Has Been Heard")]
    public class NoiseHeard : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
            => npc.soundReceiver.intensity > 0.2 ? npc.soundReceiver.intensity : 0;
    }
}