using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Navigation
{
    [DefaultExecutionOrder(-999)]
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

        public static bool RaycastGameObject(out GameObject go, LayerMask mask)
        {
            var rayOrigin = _camera.ScreenPointToRay(Input.mousePosition);

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;
            // Check if our raycast has hit anything
            var success = Physics.Raycast(rayOrigin, out hit, float.PositiveInfinity, mask);
            go = success ? hit.transform.gameObject : null;
            return success;
        }

        public static int IndexFromMask(int mask)
        {
            for (var i = 0; i < 32; ++i)
            {
                if (1 << i == mask)
                    return i;
            }

            return -1;
        }

        public static bool InLayer(LayerMask mask, int layer)
        {
            return (mask == (mask | (1 << layer)));
        }
    }

    [Serializable]
    public struct LayerMasks
    {
        public string Name;
        public LayerMask Layers;
    }
}