using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame.SceneManagement
{

    /// <summary>
    /// シーンの管理を行うクラス。シーンの読み込みや、シーン間でのデータの受け渡しをサポートする。
    /// </summary>
    public static class GameSceneManager
    {
        /// <summary>
        /// シーンを読み込む。シーン名を指定して呼び出すと、そのシーンがロードされる。シーン間でデータを受け渡す場合は、事前にSetDataメソッドでデータを設定しておくことができる。
        /// </summary>
        /// <param name="sceneName">読み込むシーンの名前</param>
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(sceneName, mode);
        }
        
        /// <summary>
        /// シーン間でデータを設定するためのメソッド。キーと値を指定してデータを保存する。
        /// </summary>
        /// <typeparam name="T">保存する値の型</typeparam>
        /// <param name="key">データのキー</param>
        /// <param name="value">保存する値</param>
        public static void SetData<T>(string key, T value)
        {
            SceneTransferData.Set(key, value);
        }
        
        /// <summary>
        /// シーン間でデータを取得するためのメソッド。キーを指定してデータを取得する。
        /// </summary>
        /// <typeparam name="T">取得する値の型</typeparam>
        /// <param name="key">データのキー</param>
        /// <param name="value">取得した値</param>
        /// <returns>値が存在する場合はtrue、存在しない場合はfalse</returns>
        public static bool TryGetData<T>(string key, out T value)
        {
            return SceneTransferData.TryGet(key, out value);
        }
        
        /// <summary>
        /// シーン間でデータをクリアするためのメソッド。すべてのデータを削除する。
        /// </summary>
        public static void ClearData()
        {
            SceneTransferData.Clear();
        }
    }
}
