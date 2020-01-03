using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class MultiplayerLauncher : MonoBehaviourPunCallbacks
    {
        public delegate void OnSceneLoaded();
        bool isLoading;

        public static MultiplayerLauncher singleton;

        public PunLogLevel logLevel;
        public uint gameVersion = 1;

        public SO.GameEvent onConnectingToMaster;
        public SO.GameEvent onJoinedRoom;
        public SO.BoolVariable isConnected;
        public SO.BoolVariable isMultiplayer;

        List<RoomInfo> curRoomsList;

        #region Init
        private void Awake()
        {
            if (!singleton)
            {
                singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }

        private void Start()
        {
            isConnected.value = false;
            Debug.Log("Connecting to master...");
            ConnectToServer();
        }

        void ConnectToServer()
        {
            PhotonNetwork.LogLevel = logLevel;
            // PhotonNetwork.autoJoinLobby is gone.
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion.ToString();
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion

        #region Photon Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected Successfully");
            isConnected.value = true;
            onConnectingToMaster.Raise();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnected.value = false;
            onConnectingToMaster.Raise();
            Debug.Log("Disconnected with reason " + cause);
        }

        /// <summary>
        /// Runs when the room is created
        /// Only runs on the player who created the room
        /// </summary>
        public override void OnCreatedRoom()
        {
            Room r = ScriptableObject.CreateInstance<Room>();

            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("scene", out object sceneName);
            r.sceneName = (string)sceneName;
            r.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManagers.GetResourcesManager().curRoom.value = r;
            Debug.Log("Room " + r.roomName + " created Successfully");
        }

        /// <summary>
        /// Runs on everyone who join a room (even after creating)
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room");
            onJoinedRoom.Raise();

            InstanciateMultiplayerManager();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            StartCoroutine(RoomCheck());
        }

        IEnumerator RoomCheck()
        {
            yield return new WaitForSeconds(3);
            MatchMakingManager m = MatchMakingManager.singleton;

            Debug.Log("Found " + curRoomsList.Count + " rooms");
            for (int i = 0; i < curRoomsList.Count; i++)
            {
                m.AddMatch();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Lobby Update Received");
            curRoomsList = roomList;
        }
        #endregion

        #region Manager Methods
        public void JoinLobby() // Its called by an event
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        void InstanciateMultiplayerManager()
        {
            PhotonNetwork.Instantiate("MultiplayerManager", Vector3.zero, Quaternion.identity);
        }

        public void CreateRoom(RoomButton b)
        {
            if (isMultiplayer.value)
            {
                if (!isConnected.value)
                {
                    //TODO: Make stuff for when not connected but still on multiplayer
                }
                else
                {
                    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        {"scene",b.scene }
                    };

                    RoomOptions roomOptions = new RoomOptions
                    {
                        MaxPlayers = 4,
                        CustomRoomProperties = properties
                    };

                    PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
                }
            }
            else // is playing solo
            {
                Room r = ScriptableObject.CreateInstance<Room>();
                r.sceneName = b.scene;
                GameManagers.GetResourcesManager().curRoom.Set(r);
            }
        }

        void LoadMainMenu()
        {
            StartCoroutine(LoadScene("MainMenu", OnMainMenu));
        }

        public void LoadCurrentRoom() // Its called by an event
        {
            if (isConnected.value)
            {
                MultiplayerManager.singleton.BroadcastSceneChange();
            }
            else
            {
                // Run current scene without callback
                LoadCurrentSceneActual();
            }
        }

        public void LoadCurrentSceneActual(OnSceneLoaded callback = null)
        {
            Room r = GameManagers.GetResourcesManager().curRoom.value;
            if (!isLoading)
            {
                isLoading = true;
                Debug.Log("Loading scene: " + r.sceneName);
                StartCoroutine(LoadScene(r.sceneName, callback));
            }
        }

        IEnumerator LoadScene(string targetLevel, OnSceneLoaded callback = null)
        {
            yield return SceneManager.LoadSceneAsync(targetLevel, LoadSceneMode.Single);
            isLoading = false;
            if (callback != null)
                callback.Invoke();
        }
        #endregion

        #region Setup Methods
        void OnMainMenu()
        {
            isConnected.value = PhotonNetwork.IsConnected;
            if (isConnected.value)
                OnConnectedToMaster();
            else
                ConnectToServer();
        }
        #endregion
    }
}