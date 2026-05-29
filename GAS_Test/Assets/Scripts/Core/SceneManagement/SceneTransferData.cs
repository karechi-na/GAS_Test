using System.Collections.Generic;

namespace MyGame.SceneManagement
{
    internal static class SceneTransferData
    {
        private static readonly Dictionary<string, object> data = new();

        public static void Set<T>(string key, T value)
        {
            data[key] = value;
        }

        public static bool TryGet<T>(string key, out T value)
        {
            if (data.TryGetValue(key, out object obj) && obj is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public static void Clear()
        {
            data.Clear();
        }
    }
}