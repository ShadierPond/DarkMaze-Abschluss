using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Player On Sight Consideration", menuName = "UtilityAI/Considerations/Player On Sight")]
    public class PlayerOnSight : Consideration
    {
        /// <summary>
        /// A method that overrides the ScoreConsideration method of the base class and returns a score based on whether the npc can see the player or not.
        /// </summary>
        /// <remarks>
        /// It requires that the npc has a stats component and a player field in the stats component.
        /// This method gets the positions of the npc and the player and performs a raycast from the npc's position to the player's position using Physics.Raycast. It also calculates the angle between the npc's forward direction and the direction to the player using Vector3.Angle. It also checks if the angle is less than half of the npc's sight angle using Quaternion.Euler. If the raycast hits nothing or something other than the player, or if the angle is greater than or equal to half of the npc's sight angle, it returns 0 as the score. Otherwise, it returns 1 as the score.
        /// </remarks>
        /// <param name="npc">
        /// NonPlayerCharacter - the npc that is scoring the consideration.
        /// </param>
        /// <returns>
        /// float - a score between 0 and 1 that represents how likely the npc is to perform this action based on whether the npc can see the player or not.
        /// </returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
        {
            // Get a reference to the player field of the stats component of the npc.
            var player = npc.stats.player;
            // If the player field is null, return 0 as the score.
            if(player == null)
                return 0f;
            // Get a reference to the position of the player's transform component.
            var playerPosition = player.transform.position;
            // Get a reference to the position of the npc's transform component.
            var npcPosition = npc.transform.position;

            // Perform a raycast from the npc's position to the direction of the player's position minus the npc's position, with a maximum distance of the npc's sight range. Store the hit information in a variable named hit using Physics.Raycast.
            Physics.Raycast(npcPosition, playerPosition - npcPosition, out var hit, npc.stats.sightRange);
            // Calculate the angle between the npc's forward direction and the direction of the player's position minus the npc's position using Vector3.Angle. Store it in a variable named angle.
            var angle = Vector3.Angle(npc.transform.forward, playerPosition - npcPosition);
            // Check if the angle is less than half of the npc's sight angle by rotating a quaternion by half of the sight angle around the y-axis and getting its euler angles. Store it in a variable named onSight using Quaternion.Euler and comparing it with angle.
            var onSight = angle < Quaternion.Euler(0, npc.stats.sightAngle / 2f, 0).eulerAngles.y;
            // If the raycast hits nothing or something other than the player, return 0 as the score.
            if(hit.collider is null)
                return 0f;
            // If the raycast hits the player and onSight is true, return 1 as the score.
            if (hit.collider.gameObject == player && onSight)
                return 1f;
            // Otherwise, return 0 as the score.
            return 0f;
        }
    }
}