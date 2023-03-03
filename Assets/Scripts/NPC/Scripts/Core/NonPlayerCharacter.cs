using System;
using System.Collections;
using NPC.UtilityAI;
using UnityEngine;
using Action = NPC.UtilityAI.Action;

namespace NPC.Core
{
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

        public void DoWork(int time)
        {
            StartCoroutine(WorkCoroutine(time));
        }
        
        public void DoSleep(int time)
        {
            StartCoroutine(SleepCoroutine(time));
        }
        
        public void DoEat(int time)
        {
            StartCoroutine(EatCoroutine(time));
        }
        
         private IEnumerator WorkCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            Debug.Log("Work done");
            
            //TODO: Add some logic
            
            // Decide what to do next
            OnFinishedAction();
        }
        
        private IEnumerator SleepCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            Debug.Log("Sleep done");
            
            //TODO: Add some logic
            
            // Decide what to do next
            OnFinishedAction();
        }
        
        private IEnumerator EatCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            Debug.Log("Eat done");
            
            //TODO: Add some logic
            
            // Decide what to do next
            OnFinishedAction();
        }
        

        #endregion
    }
}
