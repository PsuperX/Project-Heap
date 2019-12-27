using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/State Actions Holder")]
    public class StateActionsUpdater : StateActions
    {
        public StateActions[] inputs;

        public override void Execute(StateManager states)
        {
            if (inputs != null)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i].Execute(states);
                }
            }
        }
    }
}