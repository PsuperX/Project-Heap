using UnityEngine;

namespace SA.MainMenu
{
    public class InventoryManager : MonoBehaviour
    {
        public Transform cam;

        public Transform[] cameraPositions;
        int camIndex = 0;

        float t;
        public float speed;
        Vector3 startPos;
        Quaternion startRot;
        Vector3 endPos;
        Quaternion endRot;
        bool isInit;
        bool isLerping;

        private void OnEnable()
        {
            cam.localPosition = cameraPositions[0].localPosition;
            cam.localRotation = cameraPositions[0].localRotation;
        }

        public void AssignCamIndex(int index)
        {
            camIndex = index;
            isInit = false;
            isLerping = true;
        }

        private void Update()
        {
            MoveCameraToPosition();
        }

        void MoveCameraToPosition()
        {
            if (!isLerping)
                return;

            if (!isInit)
            {
                startPos = cam.localPosition;
                startRot = cam.localRotation;
                endPos = cameraPositions[camIndex].localPosition;
                endRot = cameraPositions[camIndex].localRotation;

                t = 0;
                isInit = true;
            }

            t += Time.deltaTime * speed;
            if (t >= 1)
            {
                t = 1;
                isInit = false;
                isLerping = false;
            }

            Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);
            Quaternion targetRot = Quaternion.Slerp(startRot, endRot, t);

            cam.localPosition = targetPos;
            cam.localRotation = targetRot;
        }
    }
}