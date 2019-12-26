using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Is Grounded")]
    public class IsGrounded : StateActions
    {
        public override void Execute(StateManager states)
        {
            Vector3 origin = states.mTransform.position;
            origin.y += .7f;
            Vector3 dir = Vector3.down;
            float dst = 1.4f;
            RaycastHit hit;
            Debug.DrawRay(origin, dir * dst);
            if (Physics.Raycast(origin, dir, out hit, dst))
            {
                Vector3 targetPosition = hit.point;
                states.mTransform.position = targetPosition;
            }
        }
    }
}