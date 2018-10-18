using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public static class Pool
    {
        private static Dictionary<string, Stack<GameObject>> _objectPool;
    
        static Pool()
        {
            _objectPool = new Dictionary<string, Stack<GameObject>>();
        }

        public static void ReturnToPool(string key, GameObject obj)
        {
            CheckPoolExists(key);
            obj.SetActive(false);
            _objectPool[key].Push(obj);
        }

        public static void ReturnToPool(string key, List<GameObject> obj)
        {
            CheckPoolExists(key);
            foreach (var item in obj)
            {
                item.SetActive(false);
                _objectPool[key].Push(item);
            }
        }

        public static GameObject Spawn(string key)
        {
            CheckPoolExists(key);
            GameObject go = _objectPool[key].Count > 0 ? _objectPool[key].Pop() : null;
            go?.SetActive(true);
            return go;
        }
    
        private static void CheckPoolExists(string key)
        {
            if (!_objectPool.ContainsKey(key))
            {
                _objectPool[key] = new Stack<GameObject>();
            }
        }
    }
}