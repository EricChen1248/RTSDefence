using System;
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
            try
            {
                var go = ObjectPool[key].Pop();
                go.SetActive(true);
                return go;
            }
            catch (InvalidOperationException)
            {
            }
            catch (KeyNotFoundException)
            {
            }

            return null;
        }

        private static void CheckPoolExists(string key)
        {
            if (!ObjectPool.ContainsKey(key)) ObjectPool[key] = new Stack<GameObject>();
        }
    }
}