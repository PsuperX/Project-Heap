using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Move Forward")]
    public class MoveForward : StateActions
    {
        public float frontRayOffset = .5f;
        public float movementSpeed = 4;
        public float adaptSpeed = 10;

        public override void Execute(StateManager states)
        {
            float frontY = 0;
            Vector3 origin = states.mTransform.position + (states.mTransform.forward * frontRayOffset);
            origin.y += .5f;
            Debug.DrawRay(origin, Vector3.down, Color.red, .01f, false);
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 1, states.ignoreLayers))
            {
                float y = hit.point.y;
                frontY = y - states.mTransform.position.y;
            }

            Vector3 currentVelocity = states.rigid.velocity;
            Vector3 targetVelocity = states.mTransform.forward * states.movementValues.moveAmount * movementSpeed;

            if (states.isGrounded)
            {
                float moveAmount = states.movementValues.moveAmount;

                if (moveAmount > .1f)
                {
                    states.rigid.isKinematic = false;
                    states.rigid.drag = 0;
                    if (Mathf.Abs(frontY) > .02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + .2f : frontY) * movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);

                    if (abs > .02f)
                    {
                        states.rigid.isKinematic = false;
                        targetVelocity.y = 0;
                        states.rigid.drag = 4;
                    }
                }
            }
            else
            {
                states.rigid.isKinematic = false;
                states.rigid.drag = 0;
                targetVelocity.y = currentVelocity.y;
            }

            Debug.DrawRay(states.mTransform.position + Vector3.up * .2f, targetVelocity, Color.green, .01f, false);
            states.rigid.velocity = Vector3.Lerp(currentVelocity, targetVelocity, states.delta * adaptSpeed);

            #region Junk
            /*
            if (states.movementValues.moveAmount > .1f)
                states.rigid.drag = 0;
            else
                states.rigid.drag = 4;

            float targetSpeed = states.isCrouching ? crouchSpeed : movementSpeed;

            Vector3 velocity = states.mTransform.forward * states.movementValues.moveAmount * targetSpeed;
            states.rigid.velocity = velocity;
            */
            #endregion
        }
    }
}