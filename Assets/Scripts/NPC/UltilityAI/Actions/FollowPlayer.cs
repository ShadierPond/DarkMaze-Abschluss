using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "Follow Player Action", menuName = "UtilityAI/Actions/Follow Player")]
    public class FollowPlayer : Action
    {
        public override void Execute(NonPlayerCharacter npc)
            => npc.FollowPlayer();
    }
}