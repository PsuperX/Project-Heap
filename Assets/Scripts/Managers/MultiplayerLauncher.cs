using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class MultiplayerLauncher : MonoBehaviour
    {
        delegate void OnSceneLoaded();
        bool isLoading;

        public static MultiplayerLauncher singleton;

        private void Awake()
        {
            if (!singleton)
                singleton = this;
            else
                Destroy(this.gameObject);
        }

        public void LoadCurrentRoom()
        {
            Room r = GameManagers.GetResourcesManager().curRoom.value;

            if (!isLoading)
            {
                isLoading = true;
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
    }
}