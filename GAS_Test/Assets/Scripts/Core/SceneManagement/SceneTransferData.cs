using System.Collections.Generic;

namespace MyGame.SceneManagement
{
    /// <summary>
    /// アセンブリを分け直接アクセスできないようにしたシーン間のデータ転送用クラス。
    /// </summary>
    internal static class SceneTransferData
    {
        // データを保持するための辞書。キーは文字列、値はオブジェクトとして保存される。
        private static readonly Dictionary<string, object> data = new();

        /// <summary>
        /// 値を保存するためのメソッド。キーと値を指定して、データを辞書に保存する。
        /// </summary>
        /// <typeparam name="T">保存する値の型</typeparam>
        /// <param name="key">データのキー</param>
        /// <param name="value">保存する値</param>
        public static void Set<T>(string key, T value)
        {
            data[key] = value;
        }
        
        /// <summary>
        /// 値を取得するためのメソッド。キーを指定して、データを辞書から取得する。
        /// </summary>
        /// <typeparam name="T">取得する値の型</typeparam>
        /// <param name="key">データのキー</param>
        /// <param name="value">取得した値</param>
        /// <returns>値が存在する場合はtrue、存在しない場合はfalse</returns>
        public static bool TryGet<T>(string key, out T value)
        {
            // データが存在し、かつ指定された型にキャストできる場合は値を返す。そうでない場合はデフォルト値を返す。
            if (data.TryGetValue(key, out object obj) && obj is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }
        
        /// <summary>
        /// データをクリアするためのメソッド。辞書内のすべてのデータを削除する。
        /// </summary>
        public static void Clear()
        {
            data.Clear();
        }
    }
}