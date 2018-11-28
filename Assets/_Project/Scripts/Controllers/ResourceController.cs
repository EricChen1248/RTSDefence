﻿using System;
using System.Collections.Generic;
using Scripts.GUI;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(0)]
    public class ResourceController : MonoBehaviour
    {
        public static ResourceController Instance;
        public GameObject ResouceHolderPrefab;

        [SerializeField]
        public ModelLink[] ModelLinks;
        [SerializeField]
        public ResourceLink[] ResourceLinks;

        public ResourceGroupComponent ResourceGroup;

        public static Dictionary<ResourceTypes, GameObject> ModelDictionary { get; private set; }
        public Dictionary<ResourceTypes, Sprite> SpriteDictionary { get; private set; }

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

            foreach (var resourceLink in ResourceLinks)
            {
                SpriteDictionary[resourceLink.Type] = resourceLink.Obj;
            }
            
        }

        [Serializable]
        public struct ModelLink
        {
            public ResourceTypes Resource;
            public GameObject Model;
        }

        [Serializable]
        public struct ResourceLink
        {
            public ResourceTypes Type;
            public Sprite Obj;
        }

        public static void AddResource(ResourceTypes type, int count)
        {
            ResourceCount[type] += count;
            Instance.ResourceGroup.UpdateGui(ResourceCount);
        }
    }
}
