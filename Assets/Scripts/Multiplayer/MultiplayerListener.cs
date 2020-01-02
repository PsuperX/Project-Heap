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

        public void OnPhotonInstantiate(PhotonMessageInfo info) // Interface
        {
            states = GetComponent<StateManager>();

            if (photonView.IsMine)
            {
                states.SetCurrentState(local);
                initLocalPlayer.Execute(states);
            }
            else
            {
                states.SetCurrentState(client);
                initClientPlayer.Execute(states);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // Interface
        {
            
        }
    }
}