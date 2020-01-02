using Photon.Pun;
using UnityEngine;

namespace SA
{
    public class MultiplayerManager : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        MultiplayerReferences mRef;

        public static MultiplayerManager singleton;

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);

            mRef = new MultiplayerReferences();

            if (PhotonNetwork.IsMasterClient)
                InstanciateNetworkPrint();
        }

        void InstanciateNetworkPrint()
        {
            GameObject go = PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity);
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
        #endregion
    }
}