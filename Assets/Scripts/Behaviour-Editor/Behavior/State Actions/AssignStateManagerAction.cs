using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Assign State Manager")]
    public class AssignStateManagerAction : StateActions
    {
        public StatesVariable targetVariable;

        public override void Execute(StateManager states)
        {
            targetVariable.value = states;
        }
    }
}