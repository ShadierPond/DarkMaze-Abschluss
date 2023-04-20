using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Player Close to NPC", menuName = "UtilityAI/Considerations/Player Close")]
    public class PlayerClose : Consideration
    {
        public int minDistance = 5;

        /// <summary>
        /// A method that overrides the ScoreConsideration method of the base class and returns a score based on the distance between the npc and the player.
        /// </summary>
        /// <remarks>
        /// It requires that the npc has a stats component and a player field in the stats component.
        /// This method calculates the distance between the npc's position and the player's position using Vector3.Distance and returns 1 as the score if the distance is less than minDistance, otherwise it returns 0 as the score.
        /// </remarks>
        /// <param name="npc">
        /// NonPlayerCharacter - the npc that is scoring the consideration.
        /// </param>
        /// <returns>
        /// float - a score between 0 and 1 that represents how likely the npc is to perform this action based on the distance to the player.
        /// </returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            // Calculate the distance between the npc's position and the player's position using Vector3.Distance.
            var distance = Vector3.Distance(npc.transform.position, npc.stats.player.transform.position);
            // Return 1 as the score if the distance is less than minDistance, otherwise return 0 as the score using a ternary operator.
            return distance < minDistance ? 1 : 0;
        }
        
    }
}