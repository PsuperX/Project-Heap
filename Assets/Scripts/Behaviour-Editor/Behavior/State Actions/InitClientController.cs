using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Init Client Controller")]
    public class InitClientController : StateActions
    {
        public StateActions[] actionStack;

        public override void Execute(StateManager states)
        {
            states.mTransform = states.transform;
            states.anim = states.GetComponentInChildren<Animator>();
            states.rigid = states.GetComponent<Rigidbody>();

            states.rigid.isKinematic = true;

            for (int i = 0; i < actionStack.Length; i++)
            {
                actionStack[i].Execute(states);
            }
        }
    }
}
