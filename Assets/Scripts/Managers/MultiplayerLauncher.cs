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
            Debug.Log("Connected Succesfully");
            isConnected.value = true;
            onConnectingToMaster.Raise();
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnected.value = false;
            onConnectingToMaster.Raise();
            Debug.Log("Disconnected with reason " + cause);
        }
        #endregion

        #region Manager Methods
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