using NPC;
using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Close To Player", menuName = "UtilityAI/Considerations/Close To Player")]
    public class CloseToPlayer: Consideration
    {
        /// <summary>
        /// A method that overrides the ScoreConsideration method of the base class and returns a score based on the distance between the npc and the player.
        /// </summary>
        /// <remarks>
        /// It requires that the npc has a stats component and a player field in the stats component.
        /// This method calculates the distance between the npc's position and the player's position using Vector3.Distance and returns a score that is inversely proportional to the distance and normalized by the npc's sight range. If the player field is null, it returns 0 as the score.
        /// </remarks>
        /// <param name="npc">
        /// NonPlayerCharacter - the npc that is scoring the consideration.
        /// </param>
        /// <returns>
        /// float - a score between 0 and 1 that represents how likely the npc is to perform this action based on the distance to the player.
        /// </returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            // Get a reference to the player field of the stats component of the npc.
            var player = npc.stats.player;
            // If the player field is null, return 0 as the score.
            if (player == null)
                return 0;
            // Calculate the distance between the npc's position and the player's position using Vector3.Distance.
            var distance = Vector3.Distance(npc.transform.position, player.transform.position);
            // Return a score that is inversely proportional to the distance and normalized by the npc's sight range using 1f - (distance / npc.stats.sightRange).
            return 1f - (distance / npc.stats.sightRange);
        }
    }
}