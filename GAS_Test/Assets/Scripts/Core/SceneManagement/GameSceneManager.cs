using UnityEngine.SceneManagement;

namespace MyGame.SceneManagement
{

    public static class GameSceneManager
    {
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static void SetData<T>(string key, T value)
        {
            SceneTransferData.Set(key, value);
        }

        public static bool TryGetData<T>(string key, out T value)
        {
            return SceneTransferData.TryGet(key, out value);
        }
    }
}
