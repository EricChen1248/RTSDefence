using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    public class NavigationBaker : MonoBehaviour
    {
        public static NavigationBaker Instance { get; private set;}
        private NavMeshSurface _surface;
        // Use this for initialization
        public void Start ()
        {
            Instance = this;
            _surface = GetComponent<NavMeshSurface>();
            RebuildNavMesh();
        }
        
        private void FixedUpdate()
        {
        }

        public void RebuildNavMesh()
        {
            _surface.BuildNavMesh();
        }
    }
}
