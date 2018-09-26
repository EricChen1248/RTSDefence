using UnityEngine;
using UnityEngine.AI;

namespace Navigation
{
    public class NavigationBaker : MonoBehaviour
    {
        public static NavigationBaker Instance { get; private set; }

        public void Start ()
        {
            Instance = this;
            Rebuild();
        }

        public void Rebuild()
        {
            foreach (var surface in GetComponentsInChildren<NavMeshSurface>())
            {
                surface.BuildNavMesh();
            }
        }
    }
}
