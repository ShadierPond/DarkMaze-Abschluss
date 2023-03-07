using System.Collections;
using NPC.UtilityAI;
using Sound.Detection;
using UnityEngine;
using Action = NPC.UtilityAI.Action;

namespace NPC.Core
{
    [RequireComponent(typeof(MoveController)), RequireComponent(typeof(AIBrain)), RequireComponent(typeof(Stats)), RequireComponent(typeof(SoundReciever))]
    public class NonPlayerCharacter : MonoBehaviour
    {
        public MoveController mover;
        public AIBrain AIBrain;
        public Action[] actionsAvailable;
        public Stats stats;
        public SoundReciever soundReciever;
    
        public void Start()
        {
            mover = GetComponent<MoveController>();
            AIBrain = GetComponent<AIBrain>();
            stats = GetComponent<Stats>();
            soundReciever = GetComponent<SoundReciever>();
        }

        private void Update()
        {
            if (!AIBrain.finishedDeciding) 
                return;
            AIBrain.finishedDeciding = false;
            AIBrain.bestAction.Execute(this);
        }

        private void OnFinishedAction()
            => AIBrain.DecideBestAction(actionsAvailable);

        #region Coroutines
        
        public void FollowPlayer() 
            => StartCoroutine(FollowPlayerCoroutine());

        private IEnumerator FollowPlayerCoroutine()
        {
            mover.MoveTo(stats.player.transform.position);
            yield return new WaitForSeconds(0.5f);
            //while(mover.agent.remainingDistance > 0.5f)
            //{
            //    yield return new WaitForSeconds(0.5f);
            //}
            OnFinishedAction();
        }
        
        public void Wander()
            => StartCoroutine(WanderCoroutine());

        private IEnumerator WanderCoroutine()
        {
            //mover.MoveTo(stats.mazeGenerator.GetRandomPositionInMaze());
            yield return new WaitForSeconds(0.5f);
            //while(mover.agent.remainingDistance > 0.5f)
            //{
            //    yield return new WaitForSeconds(0.5f);
            //}
            OnFinishedAction();
        }
        
        public void GoToSoundPosition()
            => StartCoroutine(GoToSoundPositionCoroutine());

        private IEnumerator GoToSoundPositionCoroutine()
        {
            mover.MoveTo(soundReciever.lastSoundPosition);
            yield return new WaitForSeconds(0.5f);
            //while(mover.agent.remainingDistance > 0.5f)
            //{
            //    yield return new WaitForSeconds(0.5f);
            //}
            OnFinishedAction();
        }
        #endregion
    }
}
