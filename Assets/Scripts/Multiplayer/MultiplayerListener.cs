using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace SA
{
    public class MultiplayerListener : MonoBehaviourPun, IPunInstantiateMagicCallback, IPunObservable
    {
        public State local;
        public StateActions initLocalPlayer;
        public State client;
        public StateActions initClientPlayer;

        StateManager states;

        Transform mTransform;

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            states = GetComponent<StateManager>();
            mTransform = transform;

            if (photonView.IsMine)
            {
                states.isLocal = true;
                states.SetCurrentState(local);
                initLocalPlayer.Execute(states);
            }
            else
            {
                states.isLocal = false;
                states.SetCurrentState(client);
                initClientPlayer.Execute(states);
                states.multiplayerListener = this;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // Interface
        {
            if (stream.IsWriting)
            {
                stream.SendNext(mTransform.position);
                stream.SendNext(mTransform.rotation);
                
                stream.SendNext(states.isAiming);
                stream.SendNext(states.movementValues.horizontal);
                stream.SendNext(states.movementValues.vertical);
            }
            else
            {
                Vector3 position = (Vector3)stream.ReceiveNext();
                Quaternion rotation = (Quaternion)stream.ReceiveNext();

                ReceivePositionRotation(position, rotation);

                states.isAiming = (bool)stream.ReceiveNext();
                states.movementValues.horizontal = (float)stream.ReceiveNext();
                states.movementValues.vertical = (float)stream.ReceiveNext();
            }
        }

        #region Prediction
        Vector3 lastPosition;
        Quaternion lastRotation;
        Vector3 lastDirection;
        Vector3 targetAimPosition;

        [SerializeField] readonly float snapDistance = 4;
        [SerializeField] readonly float snapAngle = 40;
        [SerializeField] readonly float predictionSpeed = 10;
        [SerializeField] readonly float movementThreshold = .05f;
        [SerializeField] readonly float angleThreshold = .05f;

        public void Prediction()
        {
            Vector3 curPos = mTransform.position;
            Quaternion curRot = mTransform.rotation;

            float distance = Vector3.Distance(lastPosition, curPos);
            float angle = Vector3.Angle(lastRotation.eulerAngles, curRot.eulerAngles);

            if (distance > snapDistance)
                mTransform.position = lastPosition;
            if (angle > snapAngle)
                mTransform.rotation = lastRotation;

            curPos += lastDirection;
            curRot *= lastRotation;

            Vector3 targetPosition = Vector3.Lerp(curPos, lastPosition, predictionSpeed * states.delta);
            mTransform.position = targetPosition;

            Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, lastRotation, predictionSpeed * states.delta);
            mTransform.rotation = targetRotation;
        }

        void ReceivePositionRotation(Vector3 p, Quaternion r)
        {
            lastDirection = p - lastPosition;
            lastDirection /= 10; //PhotonNetwork.sendRate = 10

            if (lastDirection.magnitude > movementThreshold)
                lastDirection = Vector3.zero;

            Vector3 lastEuler = lastRotation.eulerAngles;
            Vector3 newEuler = r.eulerAngles;

            if (Quaternion.Angle(lastRotation, r) < angleThreshold)
            {
                lastRotation = Quaternion.Euler((newEuler - lastEuler) / 10);
            }
            else
            {
                lastRotation = Quaternion.identity;
            }

            lastPosition = p;
            lastRotation = r;
        }
        #endregion
    }
}