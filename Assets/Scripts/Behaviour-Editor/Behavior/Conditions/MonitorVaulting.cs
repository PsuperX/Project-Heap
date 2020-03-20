using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Monitor Vaulting")]
    public class MonitorVaulting : Condition
    {
        [Header("Raycast Options")]
        public float origin1Offset = 1;
        public float rayForwardDst = 1.2f;
        public float origin2Offset = .2f;
        public float rayHigherForwardDst = 1.5f;
        public float rayDownDst = 1.5f;

        [Header("Vaulting")]
        public float vaultOffsetPosition = 2;
        public AnimationClip vaultWalkClip;

        bool result;

        public override bool CheckCondition(StateManager state)
        {
            result = false;
            Vector3 origin = state.mTransform.position;
            origin.y += origin1Offset;
            Vector3 direction = state.mTransform.forward;

            Debug.DrawRay(origin, direction * rayForwardDst);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, rayForwardDst, state.ignoreLayers))
            {
                Vector3 origin2 = origin;
                origin2.y += origin2Offset;

                Vector3 firstHit = hit.point;
                firstHit.y -= origin1Offset;
                Vector3 normalDir = -hit.normal;

                Debug.DrawRay(origin2, direction * rayHigherForwardDst);
                if (Physics.Raycast(origin2, direction, out hit, rayHigherForwardDst, state.ignoreLayers))
                {

                }
                else
                {
                    Vector3 origin3 = origin2 + (direction * rayHigherForwardDst);
                    Debug.DrawRay(origin3, Vector3.down * rayDownDst);
                    if (Physics.Raycast(origin3, Vector3.down, out hit, rayDownDst, state.ignoreLayers))
                    {
                        // we hit ground
                        result = true;

                        state.anim.SetBool(state.hashes.isInteracting, true);
                        state.anim.CrossFade(state.hashes.vaultWalk, .2f);
                        state.vaultData.animLength = vaultWalkClip.length;
                        state.vaultData.isInit = false;
                        state.isVaulting = true;

                        state.vaultData.startPosition = state.mTransform.position;

                        Vector3 endPosition = firstHit;
                        endPosition += normalDir * vaultOffsetPosition;

                        state.vaultData.endingPosition = endPosition;
                    }
                }
            }

            return result;
        }
    }
}