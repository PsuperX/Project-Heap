using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Set Anim Bool")]
    public class SetAnimationBool : StateActions
    {
        public string target;
        public bool status;

        public override void Execute(StateManager states)
        {
            states.anim.SetBool(target, status);
        }
    }
}