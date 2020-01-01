using Photon.Pun;
using System;
using UnityEngine;

namespace SA
{
    public class NetworkPrint : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        public int photonID;
        public bool isLocal;

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MultiplayerManager mm = MultiplayerManager.singleton;

            // Not sure if this is the right ID
            photonID = photonView.OwnerActorNr;

            isLocal = photonView.IsMine;

            mm.AddNewPlayer(this);
        }
    }
}