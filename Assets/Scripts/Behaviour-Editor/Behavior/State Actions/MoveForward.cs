using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Move Forward")]
    public class MoveForward : StateActions
    {
        public float movementSpeed = 4;
        public float crouchSpeed = 2;

        public override void Execute(StateManager states)
        {
            if (states.movementValues.moveAmount > .1f)
                states.rigid.drag = 0;
            else
                states.rigid.drag = 4;

            float targetSpeed = states.isCrouching ? crouchSpeed : movementSpeed;

            Vector3 velocity = states.mTransform.forward * states.movementValues.moveAmount * targetSpeed;
            states.rigid.velocity = velocity;
        }
    }
}