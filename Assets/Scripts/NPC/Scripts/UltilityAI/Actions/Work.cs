using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "WorkAction" , menuName = "UtilityAI/Actions/Work")]
    public class Work : Action
    {
        public override void Execute(NonPlayerCharacter npc)
        {
            npc.DoWork(3);
        }
    }
}