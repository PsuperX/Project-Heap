using Photon.Pun;
using UnityEngine;

namespace SA
{
    public class MultiplayerManager : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        MultiplayerReferences mRef;
        public MultiplayerReferences GetMRef()
        {
            return mRef;
        }

        public static MultiplayerManager singleton;

        public RayBallistics ballistics;

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);

            mRef = new MultiplayerReferences();
            DontDestroyOnLoad(mRef.referencesParent.gameObject);

            InstanciateNetworkPrint();
        }

        void InstanciateNetworkPrint()
        {
            PlayerProfile profile = GameManagers.GetPlayerProfile();
            object[] data = new object[1];
            data[0] = profile.itemIds[0];

            GameObject go = PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0, data);
        }

        public void AddNewPlayer(NetworkPrint print)
        {
            print.transform.parent = mRef.referencesParent;
            PlayerHolder playerHolder = mRef.AddNewPlayer(print);

            if (print.isLocal)
            {
                mRef.localPlayer = playerHolder;
            }
        }

        #region MyCalls
        public void BroadcastSceneChange()
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("RPC_SceneChange", RpcTarget.All);
        }

        void CreateController()
        {
            mRef.localPlayer.networkPrint.InstanciateController(mRef.localPlayer.spawnPosition);
            Debug.Log("Created Controller");
        }

        public void BroadcastShootWeapon(StateManager states, Vector3 dir, Vector3 origin)
        {
            int photonId = states.photonID;
            photonView.RPC("RPC_ShootWeapon", RpcTarget.Others, photonId, dir, origin);
        }
        #endregion

        #region RPCs
        [PunRPC]
        void RPC_SceneChange()
        {
            // TODO: set spawn positions from the master
            MultiplayerLauncher.singleton.LoadCurrentSceneActual(CreateController);
        }

        [PunRPC]
        void RPC_SetSpawnPositionForPlayer(int photonID, int spawnPosition)
        {
            if (photonID == mRef.localPlayer.photonID)
            {
                mRef.localPlayer.spawnPosition = spawnPosition;
            }
        }

        [PunRPC]
        void RPC_ShootWeapon(int photonID, Vector3 dir, Vector3 origin)
        {
            if (photonID == mRef.localPlayer.photonID) // Probably not necessary
                return;

            PlayerHolder shooter = mRef.GetPlayer(photonID);
            if (shooter == null)
                return;

            ballistics.ClientShoot(shooter.states, dir, origin);
        }
        #endregion
    }
}