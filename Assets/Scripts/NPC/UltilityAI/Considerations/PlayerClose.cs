using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Player Close to NPC", menuName = "UtilityAI/Considerations/Player Close")]
    public class PlayerClose : Consideration
    {
        public int minDistance = 5;

        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            var distance = Vector3.Distance(npc.transform.position, npc.stats.player.transform.position);
            return distance < minDistance ? 1 : 0;
        }
        
    }
}