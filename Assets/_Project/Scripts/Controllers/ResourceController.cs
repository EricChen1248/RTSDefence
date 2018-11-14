using System;
using System.Collections.Generic;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(0)]
    public class ResourceController : MonoBehaviour
    {
        public static ResourceController Instance;
        public GameObject ResouceHolderPrefab;
        private static ResourceManager _manager;

        [SerializeField]
        public ModelLink[] ModelLinks;
        public static Dictionary<ResourceTypes, GameObject> ModelDictionary { get; private set; }

        public static Dictionary<ResourceTypes, int> ResourceCount { get; private set; }
        public void Start()
        {
            Instance = this;
            ModelDictionary = new Dictionary<ResourceTypes, GameObject>();
            foreach (var modelLink in ModelLinks)
            {
                ModelDictionary.Add(modelLink.Resource, modelLink.Model);
            }
            ResourceCount = new Dictionary<ResourceTypes, int>();
            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)))
            {
                ResourceCount[resourceType] = 0;
            }

            _manager = GetComponent<ResourceManager>();
        }


        [Serializable]
        public struct ModelLink
        {
            public ResourceTypes Resource;
            public GameObject Model;
        }

        public static void AddResource(ResourceTypes type, int count)
        {
            ResourceCount[type] += count;
            UpdateGUI();

        }

        private static void UpdateGUI()
        {
            foreach (var item in ResourceCount)
            {
                var key = item.Key;
                var value = item.Value;
                switch (key)
                {
                    case ResourceTypes.Rock:
                        _manager.rock = value;
                        break;
                    case ResourceTypes.Wood:
                        _manager.wood = value;
                        break;
                    case ResourceTypes.Gold:
                        _manager.gold = value;
                        break;
                    case ResourceTypes.Coal:
                        _manager.coal = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _manager.UpdateGUI();
        }
    }
}
