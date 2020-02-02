using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Mono Action From State Action")]
    public class MonoActionFromStateAction : StateActions
    {
        public Action monoAction;

        public override void Execute(StateManager states)
        {
            monoAction.Execute();
        }
    }
}