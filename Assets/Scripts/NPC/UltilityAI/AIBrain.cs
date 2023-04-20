using UnityEngine;

namespace NPC.UtilityAI
{
    public class AIBrain : MonoBehaviour
    {
        public bool finishedDeciding;
        public Action bestAction;
        [HideInInspector] public NonPlayerCharacter npc;
        
        /// <summary>
        /// Initializes the object at the start of the scene.
        /// </summary>
        /// <returns>void - This method does not return a value.</returns>
        /// <remarks>
        /// This method assigns the npc field to the NonPlayerCharacter component of the object.
        /// </remarks>
        public void Start() 
            => npc = GetComponent<NonPlayerCharacter>();

        /// <summary>
        /// Updates the state of the object every frame.
        /// </summary>
        /// <returns>void - This method does not return a value.</returns>
        /// <remarks>
        /// This method checks if the bestAction field is null. If so, it calls the DecideBestAction method 
        /// with the npc.actionsAvailable array as an argument.
        /// </remarks>
        private void Update()
        {
            if (bestAction is null)
                DecideBestAction(npc.actionsAvailable);
        }

        /// <summary>
        /// Decides the best action to take from a list of available actions.
        /// </summary>
        /// <param name="actionsAvailable">Action[] - An array of Action objects that represent the possible actions to take.</param>
        /// <returns>void - This method does not return a value.</returns>
        /// <remarks>
        /// This method uses the ScoreAction method to evaluate each action and assigns a score to it. 
        /// It then selects the action with the highest score and sets it as the bestAction field. 
        /// It also sets the finishedDeciding field to true when done.
        /// </remarks>
        public void DecideBestAction(Action[] actionsAvailable)
        {
            var score = 0f;
            var nextBestActionIndex = 0;
            for (var i = 0; i < actionsAvailable.Length; i++)
            {
                if (!(ScoreAction(actionsAvailable[i]) > score)) 
                    continue;
                nextBestActionIndex = i;
                score = actionsAvailable[i].Score;
            }
            bestAction = actionsAvailable[nextBestActionIndex];
            finishedDeciding = true;
        }
        
        /// <summary>
        /// Scores an action based on its considerations and a rescaling scheme.
        /// </summary>
        /// <param name="action">Action - An Action object that represents the action to score.</param>
        /// <returns>float - The score of the action, ranging from 0 to 1.</returns>
        /// <remarks>
        /// This method iterates over the considerations of the action and multiplies their scores. 
        /// If the score is zero, it returns immediately. Otherwise, it applies a rescaling scheme based on 
        /// Behavioral Mathematics for Game AI to adjust the score. It also sets the Score field of the action to the final score.
        /// </remarks>
        private float ScoreAction(Action action)
        {
            float score = 1;
            foreach (var consideration in action.considerations)
            {
                var considerationScore = consideration.ScoreConsideration(npc);
                score *= considerationScore;
                if (score != 0) 
                    continue;
                action.Score = 0;
                return action.Score; // No point in continuing if the score is 0
            }
            // Average the score - Rescaling Scheme (using Behavioral Mathematics for Game AI Scheme)
            var originalScore = score;
            var modFactor = 1 - (1/ action.considerations.Length);
            var makeupValue = (1 - originalScore) * modFactor;
            action.Score = originalScore + (makeupValue * originalScore);
            return action.Score;
        }
    }
}