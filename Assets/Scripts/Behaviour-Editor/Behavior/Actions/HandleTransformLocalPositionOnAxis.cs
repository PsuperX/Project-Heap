using SO;
using UnityEngine;
using UnityEngine.Animations;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Mono Actions/Handle Transform Local Position On Axis")]
    public class HandleTransformLocalPositionOnAxis : Action
    {
        public TransformVariable targetTransform;

        public BoolVariable targetBool;
        public float defaultValue;
        public float affectedValue;
        public Axis targetAxis;

        public float speed = 9;

        float actualValue;

        public override void Execute()
        {
            if (!targetTransform.value)
                return;

            float targetValue = defaultValue;

            if (targetBool.value)
            {
                targetValue = affectedValue;
            }

            actualValue = Mathf.Lerp(actualValue, targetValue, speed * Time.deltaTime);

            Vector3 targetPosition = targetTransform.value.localPosition;
            switch (targetAxis)
            {
                case Axis.None:
                    break;
                case Axis.X:
                    targetPosition.x = actualValue;
                    break;
                case Axis.Y:
                    targetPosition.y = actualValue;
                    break;
                case Axis.Z:
                    targetPosition.z = actualValue;
                    break;
                default:
                    break;
            }
            targetTransform.value.localPosition = targetPosition;
        }
    }
}