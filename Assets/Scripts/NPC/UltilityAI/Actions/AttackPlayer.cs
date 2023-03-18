using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Attack Player Action", menuName = "UtilityAI/Actions/Attack Player")]
    public class AttackPlayer : Action
    {
        public override void Execute(NonPlayerCharacter npc)
            => npc.AttackPlayer();
    }
}