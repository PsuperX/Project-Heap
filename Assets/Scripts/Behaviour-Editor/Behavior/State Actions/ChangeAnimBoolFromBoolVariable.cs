using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Anim Bool From Bool Var")]
    public class ChangeAnimBoolFromBoolVariable : StateActions
    {
        public string targetAnimVar;
        public SO.BoolVariable targetBoolVar;
        public bool reverse;

        public override void Execute(StateManager states)
        {
            if (reverse)
                states.anim.SetBool(targetAnimVar, !targetBoolVar.value);
            else
                states.anim.SetBool(targetAnimVar, targetBoolVar.value);
        }
    }
}