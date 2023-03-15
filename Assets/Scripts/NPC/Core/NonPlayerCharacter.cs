using System;
using System.Collections;
using NPC.UtilityAI;
using Sound.Detection;
using UnityEngine;
using UnityEngine.Serialization;
using Action = NPC.UtilityAI.Action;
using Random = UnityEngine.Random;

namespace NPC.Core
{
    [RequireComponent(typeof(MoveController)), RequireComponent(typeof(AIBrain)), RequireComponent(typeof(Stats)), RequireComponent(typeof(SoundReceiver))]
    public class NonPlayerCharacter : MonoBehaviour
    {
        [HideInInspector] public MoveController mover;
        [HideInInspector] public AIBrain AIBrain;
        public Action[] actionsAvailable;
        public float[] actionsScores;
        [HideInInspector] public Stats stats;
        [HideInInspector] public SoundReceiver soundReceiver;

        private void OnValidate()
        {
            if (actionsAvailable == null) 
                return;
            actionsScores = new float[actionsAvailable.Length];
        }

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
            if(stats.mazeGenerator)
                mover.MoveTo(stats.mazeGenerator.GetRandomPositionInMaze());
            else
                mover.MoveTo(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
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
            mover.Attack();
            yield return new WaitForSeconds(1f);
            OnFinishedAction();
        }
        
        #endregion
    }
}
