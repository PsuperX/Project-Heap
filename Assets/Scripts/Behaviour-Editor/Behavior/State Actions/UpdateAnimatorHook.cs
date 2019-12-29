using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Update Animator Hook")]
    public class UpdateAnimatorHook : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.animHook.Tick();
        }
    }
}