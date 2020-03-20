using UnityEngine;
using System.Collections;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Ragdoll Status")]
    public class RagdollStatus : StateActions
    {
        public bool enableRagdoll;

        public override void Execute(StateManager states)
        {
            if (enableRagdoll)
            {
                for (int i = 0; i < states.ragdollRBs.Count; i++)
                {
                    states.ragdollCols[i].isTrigger = false;
                    states.ragdollRBs[i].isKinematic = false;
                }
            }
            else
            {
                Rigidbody[] rigidbodies = states.mTransform.GetComponentsInChildren<Rigidbody>();
                states.ragdollRBs.Capacity = rigidbodies.Length;
                states.ragdollCols.Capacity = rigidbodies.Length;

                foreach (Rigidbody rigid in rigidbodies)
                {
                    if (rigid == states.rigid) // Not main rigid
                        continue;

                    states.ragdollRBs.Add(rigid);
                    rigid.isKinematic = true;

                    Collider collider = rigid.GetComponent<Collider>();
                    states.ragdollCols.Add(collider);

                    collider.isTrigger = true;
                }
            }
        }
    }
}