using System;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class RaycastHelper : MonoBehaviour
    {
        public LayerMasks[] LayerMasks;

        public static Dictionary<string, LayerMask> LayerMaskDictionary;
        private static Camera _camera;
        public void Start()
        {
            _camera = Camera.main;
            LayerMaskDictionary = new Dictionary<string, LayerMask>();
            foreach (var layerMasks in LayerMasks)
            {
                LayerMaskDictionary[layerMasks.Name] = layerMasks.Layers;
            }
        }

        public static bool TryMouseRaycast(out Vector3 results, LayerMask layerMask)
        {
            var rayOrigin = _camera.ScreenPointToRay(Input.mousePosition);

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Check if our raycast has hit anything
            var success = Physics.Raycast(rayOrigin, out hit, float.PositiveInfinity, layerMask);
            results = hit.point;
            return success;
        }

        public static bool TryMouseRaycastToGrid(out Vector3 results, LayerMask layerMask)
        {
            var success = TryMouseRaycast(out results, layerMask);
            results.x = Mathf.Round(results.x);
            results.y = Mathf.Round(results.y);
            results.z = Mathf.Round(results.z);
            return success;
        }
    
    }

    [Serializable]
    public struct LayerMasks
    {
        public string Name;
        public LayerMask Layers;
    }
}