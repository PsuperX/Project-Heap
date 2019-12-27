using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Move Forward")]
    public class MoveForward : StateActions
    {
        public float movementSpeed = 2;

        public override void Execute(StateManager states)
        {
            if (states.movementValues.moveAmount > .1f)
                states.rigid.drag = 0;
            else
                states.rigid.drag = 4;

            Vector3 velocity = states.mTransform.forward * states.movementValues.moveAmount * movementSpeed;
            states.rigid.velocity = velocity;
        }
    }
}