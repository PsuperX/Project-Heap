using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class MultiplayerLauncher : MonoBehaviourPunCallbacks
    {
        delegate void OnSceneLoaded();
        bool isLoading;

        public static MultiplayerLauncher singleton;

        public PunLogLevel logLevel;
        public uint gameVersion = 1;

        public SO.GameEvent onConnectingToMaster;
        public SO.BoolVariable isConnected;
        public SO.BoolVariable isMultiplayer;


        #region Init
        private void Awake()
        {
            if (!singleton)
                singleton = this;
            else
                Destroy(this.gameObject);
        }

        private void Start()
        {
            isConnected.value = false;
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
        /// Only runs on the player who created the run
        /// </summary>
        public override void OnCreatedRoom()
        {
            Room r = ScriptableObject.CreateInstance<Room>();

            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("scene", out object sceneName);
            r.sceneName = (string)sceneName;
            r.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManagers.GetResourcesManager().curRoom.value = r;
            Debug.Log("Room " + r.roomName + " Created Successfully");
        }

        public override void OnJoinedRoom()
        {

        }
        #endregion

        #region Manager Methods
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

        public void LoadCurrentRoom()
        {
            Room r = GameManagers.GetResourcesManager().curRoom.value;

            if (!isLoading)
            {
                isLoading = true;
                Debug.Log("Loading scene: " + r.sceneName);
                StartCoroutine(LoadScene(r.sceneName));
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