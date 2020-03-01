using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/State Actions/Assign Transform")]
    public class AssignTransformStateAction : StateActions
    {
        public SO.TransformVariable transformVariable;

        public override void Execute(StateManager states)
        {
            transformVariable.value = states.mTransform;
        }
    }
}