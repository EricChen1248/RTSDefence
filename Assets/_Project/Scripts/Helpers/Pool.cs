using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Helpers
{
    public static class Pool
    {
        private static readonly Dictionary<string, Stack<GameObject>> ObjectPool;
    
        static Pool()
        {
            ObjectPool = new Dictionary<string, Stack<GameObject>>();
        }

        public static void ReturnToPool(string key, GameObject obj)
        {
            CheckPoolExists(key);
            obj.SetActive(false);
            ObjectPool[key].Push(obj);
        }

        public static void ReturnToPool(string key, List<GameObject> obj)
        {
            CheckPoolExists(key);
            foreach (var item in obj)
            {
                item.SetActive(false);
                ObjectPool[key].Push(item);
            }
        }

        public static GameObject Spawn(string key)
        {
            CheckPoolExists(key);
            var go = ObjectPool[key].Count > 0 ? ObjectPool[key].Pop() : null;

            if (go == null) return null;
            go.SetActive(true);
            return go;

        }
    
        private static void CheckPoolExists(string key)
        {
            if (!ObjectPool.ContainsKey(key))
            {
                ObjectPool[key] = new Stack<GameObject>();
            }
        }
    }
}