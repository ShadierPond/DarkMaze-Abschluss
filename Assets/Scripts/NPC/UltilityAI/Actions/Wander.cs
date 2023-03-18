using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Wander Action", menuName = "UtilityAI/Actions/Wander")]
    public class Wander : Action
    {
        public override void Execute(NonPlayerCharacter npc)
            => npc.Wander();
    }
}