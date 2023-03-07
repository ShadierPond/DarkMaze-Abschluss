using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "GoToSoundPosition", menuName = "UtilityAI/Actions/Go To Sound Position", order = 0)]
    public class GoToSoundPosition : Action
    {
        public override void Execute(NonPlayerCharacter npc)
        {
            npc.GoToSoundPosition();
        }
    }
}