using NPC;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Close To Player", menuName = "UtilityAI/Considerations/Close To Player")]
    public class CloseToPlayer: Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            var player = npc.stats.player;
            if (player == null)
                return 0;
            var distance = Vector3.Distance(npc.transform.position, player.transform.position);
            return 1f - (distance / npc.stats.sightRange);
        }
    }
}