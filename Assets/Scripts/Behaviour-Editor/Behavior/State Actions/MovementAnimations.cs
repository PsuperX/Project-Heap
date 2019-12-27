using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Movement Animations")]
    public class MovementAnimations : StateActions
    {
        public string floatName;

        public override void Execute(StateManager states)
        {
            states.anim.SetFloat(floatName, states.movementValues.moveAmount, .2f, states.delta);
        }
    }
}