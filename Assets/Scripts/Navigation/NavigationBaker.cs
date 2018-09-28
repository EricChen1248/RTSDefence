using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Navigation
{
    [DefaultExecutionOrder(-102)]
    public class NavigationBaker : MonoBehaviour
    {
        public static NavigationBaker Instance { get; private set; }
        private NavMeshSurface[] _surface;
        
        private int _updateFrequency;
        public int Mod = 5;

        public void Start ()
        {
            Instance = this;
            _surface = GetComponents<NavMeshSurface>();
            Build();
        }

        private void Update()
        {
            if (_updateFrequency++ % Mod == 0)
            {
                Rebuild();
            }
        }   

        public void Rebuild()
        {
            foreach (var navMeshSurface in _surface)
            {
                navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
            }
        }
        

        public void Build()
        {
            foreach (var navMeshSurface in _surface)
            {
                navMeshSurface.BuildNavMesh();
            }
        }

        
    }
}
