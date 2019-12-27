using SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Actions/Mono Actions/Camera Zoom")]
    public class HandleCameraZoom : Action
    {
        [SerializeField] TransformVariable actualCameraTrans;

        [SerializeField] InputButton aimInput;

        [SerializeField] float defaultZ;
        [SerializeField] float zoomedZ;

        [SerializeField] float speed = 9;

        float actualZ;

        public override void Execute()
        {
            if (!actualCameraTrans.value)
                return;

            float targetZ = defaultZ;

            if (aimInput.isPressed)
            {
                targetZ = zoomedZ;
            }

            actualZ = Mathf.Lerp(actualZ, targetZ, speed * Time.deltaTime);

            Vector3 targetPosition = Vector3.zero;
            targetPosition.z = actualZ;
            actualCameraTrans.value.localPosition = targetPosition;
        }
    }
}