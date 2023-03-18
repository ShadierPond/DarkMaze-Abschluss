using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "GoToSoundPosition", menuName = "UtilityAI/Actions/Go To Sound Position")]
    public class GoToSoundPosition : Action
    {
        public override void Execute(NonPlayerCharacter npc)
            => npc.GoToSoundPosition();
    }
}