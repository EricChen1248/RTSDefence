using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Misc
{
    public class PathDrawer : MonoBehaviour
    {
        private static Material mat;
        private NavMeshAgent _agent;
        private LineRenderer _lr;

        // Use this for initialization
        public void Start()
        {
            if (mat == null) mat = UnityEngine.Resources.Load<Material>("Materials/TrailMat");

            _lr = gameObject.AddComponent<LineRenderer>();
            _lr.startWidth = 0.03f;
            _lr.endWidth = 0.03f;
            _lr.material = mat;

            _agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            var path = _agent.path.corners;
            if (path == null || path.Length <= 1) return;
            _lr.positionCount = path.Length;
            for (var i = 0; i < path.Length; i++) _lr.SetPosition(i, path[i]);
        }

        private void OnDestroy()
        {
            Destroy(_lr);
        }
    }
}