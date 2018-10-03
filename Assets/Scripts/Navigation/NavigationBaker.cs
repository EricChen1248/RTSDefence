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

        private AsyncOperation[] _updateOps;
        
        private int _updateFrequency;
        public int Mod = 5;

        public void Start ()
        {
            Instance = this;
            _surface = GetComponents<NavMeshSurface>();
            Build();

            _updateOps = new AsyncOperation[_surface.Length];
        }

        private void FixedUpdate()
        {
            //if (_updateFrequency++ % Mod == 0)
            {
                //Rebuild();
            }
        }   

        public void Rebuild()
        {
            for (int i = 0; i < _surface.Length; i++)
            {
                if (_updateOps[i] != null && !_updateOps[i].isDone) return;
                var op = _surface[i].UpdateNavMesh(_surface[i].navMeshData);
                op.priority = 10000;
                _updateOps[i] = op;

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
