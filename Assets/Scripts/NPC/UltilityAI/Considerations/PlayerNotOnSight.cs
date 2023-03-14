using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "No Player On Sight Consideration", menuName = "UtilityAI/Considerations/No Player On Sight")]
    public class PlayerNotOnSight : Consideration
    {
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            var player = npc.stats.player;
            if(player == null)
                return 1f;
            var playerPosition = player.transform.position;
            var npcPosition = npc.transform.position;

            Physics.Raycast(npcPosition, playerPosition - npcPosition, out var hit, npc.stats.sightRange);
            var angle = Vector3.Angle(npc.transform.forward, playerPosition - npcPosition);
            if(hit.collider is null)
                return 1f;
            if (hit.collider.gameObject == player && angle < npc.stats.sightAngle)
                return 0f;
            return 1f;
        }
    }
}