using NPC.Core;
using NPC.UtilityAI;
using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "SleepAction", menuName = "UtilityAI/Actions/Sleep")]
    public class Sleep : Action
    {
        public override void Execute(NonPlayerCharacter npc)
        {
            npc.DoSleep(3);
        }
    }
}