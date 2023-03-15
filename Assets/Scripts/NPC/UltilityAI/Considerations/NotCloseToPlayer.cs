using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Not Close To Player", menuName = "UtilityAI/Considerations/Not Close To Player")]
    public class NotCloseToPlayer : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            var player = npc.stats.player;
            if (player == null)
                return 0;
            var distance = Vector3.Distance(npc.transform.position, player.transform.position);
            return 1 - (1.3f - (distance / npc.stats.sightRange));
        }
    }
}