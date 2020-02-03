﻿using SO;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Follow Transform")]
    public class FollowTransform : Action
    {
        public TransformVariable targetTransform;
        public TransformVariable currentTransform;
        public float speed = 9;

        public FloatVariable delta;

        public override void Execute()
        {
            if (targetTransform.value == null)
                return;
            if (currentTransform.value == null)
                return;

            Vector3 targetPosition =
                Vector3.Lerp(currentTransform.value.position, targetTransform.value.position, delta.value * speed);
            currentTransform.value.position = targetPosition;
        }
    }
}
