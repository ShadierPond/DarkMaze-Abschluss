using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Player On Sight Consideration", menuName = "UtilityAI/Considerations/Player On Sight")]
    public class PlayerOnSight : Consideration
    {
        /// <summary>
        ///  Returns 1 if the player is on sight, 0 otherwise
        ///  This also takes into account the sight angle of the NPC and the sight range
        ///  The NPC will only see the player if the player is within the sight range and within the sight angle and the player is not behind a wall
        /// </summary>
        /// <param name="npc">NonPlayerCharacter - The NPC that is being considered</param>
        /// <returns>float - The score of the consideration</returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            var player = npc.stats.player;
            if(player == null)
                return 0f;
            var playerPosition = player.transform.position;
            var npcPosition = npc.transform.position;

            Physics.Raycast(npcPosition, playerPosition - npcPosition, out var hit, npc.stats.sightRange);
            var angle = Vector3.Angle(npc.transform.forward, playerPosition - npcPosition);
            var onSight = angle < Quaternion.Euler(0, npc.stats.sightAngle / 2f, 0).eulerAngles.y;
            if(hit.collider is null)
                return 0f;
            if (hit.collider.gameObject == player && onSight)
                return 1f;
            return 0f;
        }
    }
}