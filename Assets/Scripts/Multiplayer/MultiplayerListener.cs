using Photon.Pun;
using UnityEngine;

namespace SA
{
    public class MultiplayerListener : MonoBehaviourPun, IPunInstantiateMagicCallback, IPunObservable
    {
        public State local;
        public StateActions initLocalPlayer;
        public State client;
        public StateActions initClientPlayer;
        public State vaultClient;

        StateManager states;

        Transform mTransform;

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            states = GetComponent<StateManager>();
            mTransform = transform;
            object[] data = photonView.InstantiationData;
            states.photonID = (int)data[0];

            MultiplayerManager m = MultiplayerManager.singleton;
            mTransform.parent = m.GetMRef().referencesParent;

            PlayerHolder playerHolder = m.GetMRef().GetPlayer(states.photonID);
            playerHolder.states = states;

            if (photonView.IsMine)
            {
                states.isLocal = true;
                states.SetCurrentState(local);
                initLocalPlayer.Execute(states);
            }
            else
            {
                string weaponId = (string)data[1];

                states.inventory.weaponID = weaponId;

                states.isLocal = false;
                states.SetCurrentState(client);
                initClientPlayer.Execute(states);
                states.multiplayerListener = this;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // Interface
        {
            if (states.isDead)
            {
                return;
            }

            if (stream.IsWriting) // is local
            {
                stream.SendNext(mTransform.position);
                stream.SendNext(mTransform.rotation);

                stream.SendNext(states.isVaulting);
                if (!states.isVaulting)
                {
                    stream.SendNext(states.isCrouching);
                    stream.SendNext(states.isAiming);
                    stream.SendNext(states.shootingFlag);
                    states.shootingFlag = false;
                    stream.SendNext(states.reloadingFlag);
                    states.reloadingFlag = false;

                    stream.SendNext(states.movementValues.horizontal);
                    stream.SendNext(states.movementValues.vertical);
                }

                stream.SendNext(states.movementValues.aimPosition);
            }
            else // is client
            {
                Vector3 position = (Vector3)stream.ReceiveNext();
                Quaternion rotation = (Quaternion)stream.ReceiveNext();

                ReceivePositionRotation(position, rotation);

                states.isVaulting = (bool)stream.ReceiveNext();
                if (states.isVaulting)
                {
                    states.isCrouching = false;
                    states.isAiming = false;
                    states.isReloading = false;
                    states.movementValues.horizontal = 0;
                    states.movementValues.vertical = 0;
                    states.movementValues.moveAmount = 0;

                    if (!states.vaultingFlag)
                    {
                        states.vaultingFlag = true;
                        states.anim.CrossFade(states.hashes.vaultWalk, .2f);
                        states.currentState = vaultClient;
                    }
                }
                else
                {
                    if (states.vaultingFlag)
                    {
                        states.vaultingFlag = false;
                        states.currentState = client;
                    }

                    states.isCrouching = (bool)stream.ReceiveNext();
                    states.isAiming = (bool)stream.ReceiveNext();
                    states.isShooting = (bool)stream.ReceiveNext();
                    states.isReloading = (bool)stream.ReceiveNext();

                    states.movementValues.horizontal = (float)stream.ReceiveNext();
                    states.movementValues.vertical = (float)stream.ReceiveNext();
                    states.movementValues.moveAmount = Mathf.Clamp01(Mathf.Abs(states.movementValues.horizontal) + Mathf.Abs(states.movementValues.vertical));
                }

                states.movementValues.aimPosition = (Vector3)stream.ReceiveNext();
            }
        }

        #region Prediction
        Vector3 lastPosition;
        Quaternion lastRotation;
        Vector3 lastDirection;
        Vector3 targetAimPosition;

        [SerializeField] const float snapDistance = 4;
        [SerializeField] const float snapAngle = 40;
        [SerializeField] const float predictionSpeed = 10;
        [SerializeField] const float movementThreshold = .05f;
        [SerializeField] const float angleThreshold = .05f;

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