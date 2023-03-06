using System;
using System.Collections;
using NPC.UtilityAI;
using UnityEngine;
using Action = NPC.UtilityAI.Action;

namespace NPC.Core
{
    [RequireComponent(typeof(MoveController)), RequireComponent(typeof(AIBrain)), RequireComponent(typeof(Stats))]
    public class NonPlayerCharacter : MonoBehaviour
    {
        public MoveController mover;
        public AIBrain AIBrain;
        public Action[] actionsAvailable;
        public Stats stats;
    
        public void Start()
        {
            mover = GetComponent<MoveController>();
            AIBrain = GetComponent<AIBrain>();
            stats = GetComponent<Stats>();
        }

        private void Update()
        {
            if (AIBrain.finishedDeciding)
            {
                AIBrain.finishedDeciding = false;
                AIBrain.bestAction.Execute(this);
            }
        }

        public void OnFinishedAction()
        {
            AIBrain.DecideBestAction(actionsAvailable);
        }

        #region Coroutines
        
        public void FollowPlayer()
        {
            StartCoroutine(FollowPlayerCoroutine());
        }

        private IEnumerator FollowPlayerCoroutine()
        {
            mover.MoveTo(stats.player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }
        
        public void Wander()
        {
            StartCoroutine(WanderCoroutine());
        }
        
        private IEnumerator WanderCoroutine()
        {
            mover.MoveTo(stats.mazeGenerator.GetRandomPositionInMaze());
            yield return new WaitForSeconds(0.5f);
        }
        #endregion
    }
}
