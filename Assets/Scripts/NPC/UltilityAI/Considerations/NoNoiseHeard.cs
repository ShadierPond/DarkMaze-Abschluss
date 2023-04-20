using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "No Noise Heard Consideration", menuName = "UtilityAI/Considerations/No Noise Has Been Heard")]
    public class NoNoiseHeard : Consideration
    {
        /// <summary>
        /// A method that overrides the ScoreConsideration method of the base class and returns a score based on the intensity of the sound received by the npc.
        /// </summary>
        /// <remarks>
        /// It requires that the npc has a sound receiver component and an intensity field in the sound receiver component.
        /// This method returns 1 as the score if the intensity of the sound receiver component is less than or equal to 0.2, otherwise it returns 0 as the score.
        /// </remarks>
        /// <param name="npc">
        /// NonPlayerCharacter - the npc that is scoring the consideration.
        /// </param>
        /// <returns>
        /// float - a score between 0 and 1 that represents how likely the npc is to perform this action based on the intensity of the sound received.
        /// </returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
            // Return 1 as the score if the intensity of the sound receiver component is less than or equal to 0.2, otherwise return 0 as the score using a ternary operator.
            => npc.soundReceiver.intensity <= 0.2 ? 1 : 0;
    }
}