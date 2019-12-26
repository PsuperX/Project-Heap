using SO;
using System;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Inputs/InputManager")]
    public class InputManager : Action
    {
        public InputAxis horizontal;
        public InputAxis vertical;

        public float moveAmount;
        public Vector3 moveDirection;

        public TransformVariable cameraTransform;

        public StatesVariable playerStates;

        public override void Execute()
        {
            horizontal.Execute();
            vertical.Execute();

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal.value) + Math.Abs(vertical.value));

            if (cameraTransform.value != null)
            {
                moveDirection = cameraTransform.value.forward * vertical.value;
                moveDirection += cameraTransform.value.right * horizontal.value;
            }

            if (playerStates.value != null)
            {
                playerStates.value.movementValues.horizontal = horizontal.value;
                playerStates.value.movementValues.vertical = vertical.value;
                playerStates.value.movementValues.moveAmount = moveAmount;
                playerStates.value.movementValues.moveDirection = moveDirection;
            }
        }
    }
}
