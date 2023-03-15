using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI
{
    public class AIBrain : MonoBehaviour
    {
        public bool finishedDeciding;
        public Action bestAction;
        [HideInInspector] public NonPlayerCharacter npc;
        
        public void Start() 
            => npc = GetComponent<NonPlayerCharacter>();

        private void Update()
        {
            if (bestAction is null)
                DecideBestAction(npc.actionsAvailable);
        }

        /// <summary>
        /// This method will loop through every action and determine which one is the best to take.
        /// Best action is determined by the highest score.
        /// </summary>
        /// <param name="actionsAvailable">List of available Actions for the NPC</param>
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
                npc.actionsScores[i] = score;
            }
            bestAction = actionsAvailable[nextBestActionIndex];
            finishedDeciding = true;
        }
        
        /// <summary>
        /// This method will loop through every action and score it based on the NPC's Considerations
        /// Score all the considerations for each action
        /// Average consideration score is the score for the action.
        /// </summary>
        /// <param name="action">The Action to be evaluated</param>
        /// <returns name="score"> The calculated score for the action</returns>
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