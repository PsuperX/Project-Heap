using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Is Grounded")]
    public class IsGrounded : StateActions
    {
        public float groundedDis = .8f;
        public float onAirDis = .85f;

        public override void Execute(StateManager states)
        {
            Vector3 origin = states.mTransform.position;

            origin.y += .7f;
            Vector3 dir = Vector3.down;
            float dis = groundedDis;
            if (!states.isGrounded)
                dis = onAirDis;

            Debug.DrawRay(origin, dir * dis);
            if (Physics.SphereCast(origin, .3f, dir, out RaycastHit hit, dis, states.ignoreLayers))
                states.isGrounded = true;
            else
                states.isGrounded = false;
        }
    }
}