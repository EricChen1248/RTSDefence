using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components
{
    public class PathDrawer : MonoBehaviour
    {

        private NavMeshAgent _agent;
        private LineRenderer _lr;
    
        // Use this for initialization
        public void Start ()
        {
            _lr = gameObject.AddComponent<LineRenderer>();
            _lr.startWidth = 0.01f;
            _lr.endWidth = 0.01f;
            _lr.material = UnityEngine.Resources.Load<Material>("Materials/TrailMat");

            _agent = GetComponent<NavMeshAgent>();
        }
	
        // Update is called once per frame
        public void FixedUpdate ()
        {
            var path = _agent.path.corners;
            if (path == null || path.Length <= 1) return;
            _lr.positionCount = path.Length;
            for (var i = 0; i < path.Length; i++)
            {
                _lr.SetPosition(i, path[i]);
            }
        }

        private void OnDestroy()
        {
            Destroy(_lr);
        }
    }
}
