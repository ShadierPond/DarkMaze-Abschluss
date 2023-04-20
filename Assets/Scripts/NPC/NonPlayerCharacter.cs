using System.Collections;
using NPC.UtilityAI;
using UnityEngine;
using Management.Sound.Detection;
using Action = NPC.UtilityAI.Action;
using Random = UnityEngine.Random;

namespace NPC
{
    [RequireComponent(typeof(MoveController)), RequireComponent(typeof(AIBrain)), RequireComponent(typeof(Stats)), RequireComponent(typeof(SoundReceiver))]
    public class NonPlayerCharacter : MonoBehaviour
    {
        [HideInInspector] public MoveController mover;
        [HideInInspector] public AIBrain AIBrain;
        public Action[] actionsAvailable;
        [HideInInspector] public Stats stats;
        [HideInInspector] public SoundReceiver soundReceiver;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Die = Animator.StringToHash("Die");

        /// <summary>
        /// This method is called before the first frame update.
        /// It requires that the game object has a MoveController, AIBrain, Stats and SoundReceiver component attached.
        /// </summary>
        /// <remarks>
        /// It assigns the MoveController, AIBrain, Stats and SoundReceiver components to the mover, AIBrain, stats and soundReceiver fields respectively.
        /// </remarks>
        public void Start()
        {
            mover = GetComponent<MoveController>();
            AIBrain = GetComponent<AIBrain>();
            stats = GetComponent<Stats>();
            soundReceiver = GetComponent<SoundReceiver>();
        }

        /// <summary>
        /// This method is called once per frame.
        /// </summary>
        /// <remarks>
        /// It checks if the AIBrain has finished deciding the best action to take. If not, it returns early.
        /// If yes, it sets the finishedDeciding flag to false and executes the best action on this game object.
        /// </remarks>
        private void Update()
        {
            if (!AIBrain.finishedDeciding) 
                return;
            AIBrain.finishedDeciding = false;
            AIBrain.bestAction.Execute(this);
        }

        /// <summary>
        /// This method is called when an action is finished executing.
        /// </summary>
        /// <remarks>
        /// It invokes the DecideBestAction method on the AIBrain component, passing in the actionsAvailable list as an argument.
        /// </remarks>
        private void OnFinishedAction()
            => AIBrain.DecideBestAction(actionsAvailable);
        
        /// <summary>
        /// This method reduces the health of the game object by a given amount of damage.
        /// </summary>
        /// <param name="damage">float - The amount of damage to take.</param>
        /// <remarks>
        /// It subtracts the damage from the health field of the stats component.
        /// If the health becomes zero or less, it triggers the Die animation on the animator component of the mover component.
        /// </remarks>
        public void TakeDamage(float damage)
        {
            stats.health -= damage;
            if (stats.health <= 0)
                mover.animator.SetTrigger(Die);
        }

        #region Coroutines
        
        /// <summary>
        /// This method makes the game object follow the player.
        /// </summary>
        /// <remarks>
        /// It starts a coroutine that moves the game object to the player's position and plays the walking animation.
        /// When the game object reaches the player's position, it stops the walking animation and calls the OnFinishedAction method.
        /// </remarks>
        public void FollowPlayer() 
            => StartCoroutine(FollowPlayerCoroutine());

        /// <summary>
        /// This method is a coroutine that moves the game object to the player's position and plays the walking animation.
        /// </summary>
        /// <returns>IEnumerator - The enumerator for the coroutine.</returns>
        private IEnumerator FollowPlayerCoroutine()
        {
            mover.MoveTo(stats.player.transform.position); // Move to the player's position using the mover component.
            mover.animator.SetBool(IsWalking, true); // Set the IsWalking parameter of the animator component to true.
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds.
            while(mover.agent.remainingDistance > 0.2f) // While the game object is still far from the player's position by more than 0.2 units.
                yield return null; // Wait for the next frame.
            mover.animator.SetBool(IsWalking, false); // Set the IsWalking parameter of the animator component to false.
            OnFinishedAction(); // Call the OnFinishedAction method.
        }

        /// <summary>
        /// This method makes the game object wander around randomly.
        /// </summary>
        /// <remarks>
        /// It starts a coroutine that moves the game object to a random position on a circle with radius 30 and plays the walking animation.
        /// When the game object reaches the random position, it stops the walking animation and calls the OnFinishedAction method.
        /// </remarks>
        public void Wander()
            => StartCoroutine(WanderCoroutine());

        /// <summary>
        /// This method is a coroutine that moves the game object to a random position on a circle with radius 30 and plays the walking animation.
        /// </summary>
        /// <returns>IEnumerator - The enumerator for the coroutine.</returns>
        private IEnumerator WanderCoroutine()
        {
            mover.MoveTo(new Vector3((Random.insideUnitCircle * 30).x, 0, (Random.insideUnitCircle * 30).y)); // Move to a random position on a circle with radius 30 using the mover component.
            mover.animator.SetBool(IsWalking, true); // Set the IsWalking parameter of the animator component to true.
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds.
            while(mover.agent.remainingDistance > 0.2f) // While the game object is still far from the random position by more than 0.2 units.
                yield return null; // Wait for the next frame.
            mover.animator.SetBool(IsWalking, false); // Set the IsWalking parameter of the animator component to false.
            OnFinishedAction(); // Call the OnFinishedAction method.
        }

        /// <summary>
        /// This method makes the game object go to the last sound position it heard.
        /// </summary>
        /// <remarks>
        /// It starts a coroutine that moves the game object to the last sound position stored in the soundReceiver component and plays the walking animation.
        /// When the game object reaches the last sound position, it stops the walking animation and calls the OnFinishedAction method.
        /// </remarks>
        public void GoToSoundPosition()
            => StartCoroutine(GoToSoundPositionCoroutine());

        /// <summary>
        /// Moves the game object to the last sound position detected by the sound receiver component.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a mover component and a sound receiver component attached.
        /// This method is a coroutine that can be started by calling StartCoroutine(GoToSoundPositionCoroutine()).
        /// </remarks>
        /// <returns>
        /// IEnumerator - an enumerator that can be used to control the execution of the coroutine.
        /// </returns>
        private IEnumerator GoToSoundPositionCoroutine()
        {
            // Call the MoveTo method of the mover component with the last sound position as the argument.
            mover.MoveTo(soundReceiver.lastSoundPosition);
            // Set the IsWalking parameter of the animator component to true.
            mover.animator.SetBool(IsWalking, true);
            // Wait until the remaining distance of the agent component is less than 0.5 units.
            while(mover.agent.remainingDistance > 0.5f)
                yield return null;
            // Set the IsWalking parameter of the animator component to false.
            mover.animator.SetBool(IsWalking, false);
            // Call the OnFinishedAction method to notify that the action is completed.
            OnFinishedAction();
        }
        
        /// <summary>
        /// Attacks the player by triggering the Attack1 animation of the animator component.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a mover component attached.
        /// This method calls the private coroutine AttackPlayerCoroutine to perform the attack.
        /// </remarks>
        public void AttackPlayer()
            // Start the AttackPlayerCoroutine using StartCoroutine.
            => StartCoroutine(AttackPlayerCoroutine());

        /// <summary>
        /// A private coroutine that performs the attack animation and waits for 1 second before finishing the action.
        /// </summary>
        /// <returns>
        /// IEnumerator - an enumerator that can be used to control the execution of the coroutine.
        /// </returns>
        private IEnumerator AttackPlayerCoroutine()
        {
            // Set the IsWalking parameter of the animator component to false.
            mover.animator.SetBool(IsWalking, false);
            // Set the Attack1 trigger of the animator component to true.
            mover.animator.SetTrigger(Attack1);
            // Wait for 1 second.
            yield return new WaitForSeconds(1f);
            // Call the OnFinishedAction method to notify that the action is completed.
            OnFinishedAction();
        }

        #endregion
    }
}
