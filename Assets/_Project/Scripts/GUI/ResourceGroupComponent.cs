﻿using System;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.Resources;
using UnityEngine;

namespace Scripts.GUI
{
    [DefaultExecutionOrder(1)]
    public class ResourceGroupComponent : MonoBehaviour {

        public GameObject ResourceGuiPrefab;
        private Dictionary<ResourceTypes, ResourcePanelComponent> _panelLinks;


        // Use this for initialization
        private void Start ()
        {
            ResourceController.Instance.ResourceGroup = this;

            _panelLinks = new Dictionary<ResourceTypes, ResourcePanelComponent>();
            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)))
            {
                var go = Instantiate(ResourceGuiPrefab);
                go.transform.parent = transform;
                var comp = go.GetComponent<ResourcePanelComponent>();
                _panelLinks[resourceType] = comp;
                // comp.AssignImage(ResourceController.Instance.SpriteDictionary[resourceType]);

            }
        }



        public void UpdateGui(Dictionary<ResourceTypes, int> resourceCount)
        {
            foreach (var item in resourceCount)
            {
                print(item.Key);
                _panelLinks[item.Key].UpdateText(item.Value);
            }
        }
    }
}