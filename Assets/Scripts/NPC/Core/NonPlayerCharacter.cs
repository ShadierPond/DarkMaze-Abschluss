using System.Collections;
using NPC.UtilityAI;
using Sound.Detection;
using UnityEngine;
using UnityEngine.Serialization;
using Action = NPC.UtilityAI.Action;

namespace NPC.Core
{
    [RequireComponent(typeof(MoveController)), RequireComponent(typeof(AIBrain)), RequireComponent(typeof(Stats)), RequireComponent(typeof(SoundReceiver))]
    public class NonPlayerCharacter : MonoBehaviour
    {
        public MoveController mover;
        public AIBrain AIBrain;
        public Action[] actionsAvailable;
        public Stats stats;
        public SoundReceiver soundReceiver;
    
        public void Start()
        {
            mover = GetComponent<MoveController>();
            AIBrain = GetComponent<AIBrain>();
            stats = GetComponent<Stats>();
            soundReceiver = GetComponent<SoundReceiver>();
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
            while(mover.agent.remainingDistance > 0.5f)
            { 
                if (stats.energy > 0) 
                    stats.UpdateEnergy(-5); 
                yield return new WaitForSeconds(0.5f);
            }
            OnFinishedAction();
        }
        
        public void Wander()
            => StartCoroutine(WanderCoroutine());

        private IEnumerator WanderCoroutine()
        {
            mover.MoveTo(stats.mazeGenerator.GetRandomPositionInMaze());
            yield return new WaitForSeconds(0.5f);
            while(mover.agent.remainingDistance > 0.5f)
            {
                if (stats.energy < 100) 
                    stats.UpdateEnergy(10); 
                yield return new WaitForSeconds(0.5f);
            }
            OnFinishedAction();
        }
        
        public void GoToSoundPosition()
            => StartCoroutine(GoToSoundPositionCoroutine());

        private IEnumerator GoToSoundPositionCoroutine()
        {
            mover.MoveTo(soundReceiver.lastSoundPosition);
            yield return new WaitForSeconds(0.5f);
            while(mover.agent.remainingDistance > 0.5f)
            {
                yield return new WaitForSeconds(0.5f);
            }
            OnFinishedAction();
        }
        
        public void AttackPlayer()
            => StartCoroutine(AttackPlayerCoroutine());
        
        private IEnumerator AttackPlayerCoroutine()
        {
            Debug.Log("Attacking player");
            yield return new WaitForSeconds(1f);
            OnFinishedAction();
        }
        
        #endregion
    }
}
