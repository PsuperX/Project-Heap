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
            if (Physics.Raycast(origin, dir, out hit, dst, states.ignoreLayers))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.x = states.mTransform.position.x;
                targetPosition.z = states.mTransform.position.z;
                states.mTransform.position = targetPosition;
            }
        }
    }
}