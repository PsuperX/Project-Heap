using Photon.Pun;
using System.Collections.Generic;
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

        List<PlayerHolder> playersToRespawn = new List<PlayerHolder>();
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = playersToRespawn.Count - 1; i >= 0; i--)
            {
                playersToRespawn[i].spawnTimer += deltaTime;

                if (playersToRespawn[i].spawnTimer > 5)
                {
                    playersToRespawn[i].spawnTimer = 0;
                    int ran = Random.Range(0, mRef.spawnPositions.Length);
                    Vector3 pos = mRef.spawnPositions[ran].transform.position;
                    Quaternion rot = mRef.spawnPositions[ran].transform.rotation;
                    photonView.RPC("RPC_SpawnPlayer", RpcTarget.All, playersToRespawn[i].photonID, pos, rot);
                    playersToRespawn.RemoveAt(i);
                }
            }
        }

        #region MyCalls
        public void FindSpawnPositionOnLevel()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                mRef.spawnPositions = FindObjectsOfType<SpawnPosition>();
                AssignSpawnPositions();
            }
        }

        void AssignSpawnPositions()
        {
            List<PlayerHolder> players = mRef.GetPlayers();

            for (int i = 0; i < players.Count; i++)
            {
                int index = i % mRef.spawnPositions.Length;
                SpawnPosition p = mRef.spawnPositions[i];
                photonView.RPC("RPC_BroadcastCreateControllers", RpcTarget.All,
                    players[i].photonID, p.transform.position, p.transform.rotation);
            }
        }

        public void BroadcastSceneChange()
        {
            if (PhotonNetwork.IsMasterClient)
                photonView.RPC("RPC_SceneChange", RpcTarget.All);
        }

        void LevelLoadedCallback() // After scene was loaded
        {
            Debug.Log("Level Loaded");

            if (PhotonNetwork.IsMasterClient)
                FindSpawnPositionOnLevel();
        }

        public void BroadcastShootWeapon(StateManager states, Vector3 dir, Vector3 origin)
        {
            int photonId = states.photonID;
            photonView.RPC("RPC_ShootWeapon", RpcTarget.Others, photonId, dir, origin);
        }

        public void BroadcastKillPlayer(int photonID, int shooter)
        {
            photonView.RPC("RPC_ReceiveKillPlayer", RpcTarget.MasterClient, photonID, shooter);
        }
        #endregion

        #region RPCs
        [PunRPC]
        void RPC_BroadcastCreateControllers(int photonID, Vector3 pos, Quaternion rot)
        {
            if (photonID == mRef.localPlayer.photonID)
            {
                while (MultiplayerLauncher.singleton.isLoading) { }
                mRef.localPlayer.networkPrint.InstanciateController(pos, rot);
            }
        }

        [PunRPC]
        void RPC_SceneChange()
        {
            // TODO: set spawn positions from the master
            MultiplayerLauncher.singleton.LoadCurrentSceneActual(LevelLoadedCallback);
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

        [PunRPC]
        void RPC_SpawnPlayer(int photonID, Vector3 targetPos, Quaternion targetRot)
        {
            PlayerHolder player = mRef.GetPlayer(photonID);
            if (player.states)
                player.states.SpawnPlayer(targetPos, targetRot);
        }

        [PunRPC]
        void RPC_ReceiveKillPlayer(int photonID, int shooter) // Happens on MasterClient
        {
            photonView.RPC("RPC_KillPlayer", RpcTarget.All, photonID, shooter);
            playersToRespawn.Add(mRef.GetPlayer(photonID));
        }

        [PunRPC]
        void RPC_KillPlayer(int photonID, int shooter)
        {
            PlayerHolder player = mRef.GetPlayer(photonID);
            if (player.states)
                player.states.KillPlayer();
        }
        #endregion
    }
}