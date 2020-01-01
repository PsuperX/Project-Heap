using Photon.Pun;
using UnityEngine;

namespace SA
{
    public class MultiplayerManager : MonoBehaviour, IPunInstantiateMagicCallback
    {
        MultiplayerReferences mRef;

        public static MultiplayerManager singleton;

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            singleton = this;

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
            PlayerHolder playerHolder = mRef.AddNewPlayer(print);

            if (print.isLocal)
            {
                mRef.localPlayer = playerHolder;
            }
        }
    }
}