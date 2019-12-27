using SO;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Switcher")]
    public class StateActionSwitcher : StateActions
    {
        public BoolVariable targetBool;
        public StateActions onFalseAction;
        public StateActions onTrueAction;

        public override void Execute(StateManager states)
        {
            if (targetBool.value)
            {
                if (onTrueAction)
                    onTrueAction.Execute(states);
            }
            else
            {
                if (onFalseAction)
                    onFalseAction.Execute(states);
            }
        }
    }
}