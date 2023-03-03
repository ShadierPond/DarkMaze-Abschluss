using NPC.UtilityAI;
using NPC.Core;
using UnityEngine;

namespace NPC.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "EatAction", menuName = "UtilityAI/Actions/Eat")]
    public class Eat : Action
    {
        public override void Execute(NonPlayerCharacter npc)
        {
            npc.DoEat(3);
        }
    }
}