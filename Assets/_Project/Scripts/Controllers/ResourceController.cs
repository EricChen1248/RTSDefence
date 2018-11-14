using System;
using System.Collections.Generic;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Controllers
{
    public class ResourceController : MonoBehaviour
    {
        public static ResourceController Instance;
        public GameObject ResouceHolderPrefab;

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


        }
    }
}
