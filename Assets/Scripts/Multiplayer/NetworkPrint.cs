﻿using Photon.Pun;
using UnityEngine;

namespace SA
{
    public class NetworkPrint : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        public int photonID;
        public bool isLocal;

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            MultiplayerManager mm = MultiplayerManager.singleton;

            photonID = photonView.OwnerActorNr;
            isLocal = photonView.IsMine;

            mm.AddNewPlayer(this);
        }

        public void InstanciateController(int spawnIndex)
        {
            Debug.Log("Instanciate Controller");
            GameObject inputHandler = Instantiate(Resources.Load("InputHandler")) as GameObject;

            object[] data = new object[2];
            data[0] = photonID;
            data[1] = photonView.InstantiationData[0];

            PhotonNetwork.Instantiate("MultiplayerController", Vector3.zero, Quaternion.identity, 0, data);
        }
    }
}