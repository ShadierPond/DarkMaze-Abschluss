using UnityEngine;

namespace NPC.UtilityAI.Considerations
{
    [CreateAssetMenu(fileName = "Noise Heard Consideration", menuName = "UtilityAI/Considerations/Noise Has Been Heard")]
    public class NoiseHeard : Consideration
    {
        /// <summary>
        /// A method that overrides the ScoreConsideration method of the base class and returns a score based on the intensity of the sound received by the npc.
        /// </summary>
        /// <remarks>
        /// It requires that the npc has a sound receiver component and an intensity field in the sound receiver component.
        /// This method returns the intensity of the sound receiver component as the score if it is greater than 0.2, otherwise it returns 0 as the score.
        /// </remarks>
        /// <param name="npc">
        /// NonPlayerCharacter - the npc that is scoring the consideration.
        /// </param>
        /// <returns>
        /// float - a score between 0 and 1 that represents how likely the npc is to perform this action based on the intensity of the sound received.
        /// </returns>
        public override float ScoreConsideration(NonPlayerCharacter npc)
            // Return the intensity of the sound receiver component as the score if it is greater than 0.2, otherwise return 0 as the score using a ternary operator.
            => npc.soundReceiver.intensity > 0.2 ? npc.soundReceiver.intensity : 0;
    }
}