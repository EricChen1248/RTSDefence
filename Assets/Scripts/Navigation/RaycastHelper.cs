using UnityEngine;

namespace Navigation
{
    public static class RaycastHelper
    {
        private static readonly Camera Camera;
        static RaycastHelper()
        {
            Camera = Camera.main;
        }

        public static bool TryMouseRaycast(out Vector3 results)
        {
            var rayOrigin = Camera.ScreenPointToRay(Input.mousePosition);

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Check if our raycast has hit anything
            var success = Physics.Raycast(rayOrigin, out hit);
            results = hit.point;
            return success;
        }
    
    }
}