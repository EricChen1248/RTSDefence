using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

// Build and update a localized navmesh from the sources marked by NavMeshSourceTag
namespace External_Libraries.NavMeshComponents.Scripts
{
    [DefaultExecutionOrder(-102)]
    public class LocalNavMeshBuilder : MonoBehaviour
    {
        // The center of the build
        public Transform MTracked;

        // The size of the build bounds
        public Vector3 MSize = new Vector3(80.0f, 20.0f, 80.0f);

        private NavMeshData _mNavMesh;
        private AsyncOperation _mOperation;
        private NavMeshDataInstance _mInstance;
        private List<NavMeshBuildSource> _mSources = new List<NavMeshBuildSource>();

        private IEnumerator Start()
        {
            while (true)
            {
                UpdateNavMesh(true);
                yield return _mOperation;
            }
        }

        private void OnEnable()
        {
            // Construct and add navmesh
            _mNavMesh = new NavMeshData();
            _mInstance = NavMesh.AddNavMeshData(_mNavMesh);
            if (MTracked == null)
                MTracked = transform;
            UpdateNavMesh(false);
        }

        private void OnDisable()
        {
            // Unload navmesh and clear handle
            _mInstance.Remove();
        }

        private void UpdateNavMesh(bool asyncUpdate = false)
        {
            NavMeshSourceTag.Collect(ref _mSources);
            var defaultBuildSettings = NavMesh.GetSettingsByID(0);
            var bounds = QuantizedBounds();

            if (asyncUpdate)
                _mOperation = NavMeshBuilder.UpdateNavMeshDataAsync(_mNavMesh, defaultBuildSettings, _mSources, bounds);
            else
                NavMeshBuilder.UpdateNavMeshData(_mNavMesh, defaultBuildSettings, _mSources, bounds);
        }

        private static Vector3 Quantize(Vector3 v, Vector3 quant)
        {
            var x = quant.x * Mathf.Floor(v.x / quant.x);
            var y = quant.y * Mathf.Floor(v.y / quant.y);
            var z = quant.z * Mathf.Floor(v.z / quant.z);
            return new Vector3(x, y, z);
        }

        private Bounds QuantizedBounds()
        {
            // Quantize the bounds to update only when theres a 10% change in size
            var center = MTracked ? MTracked.position : transform.position;
            return new Bounds(Quantize(center, 0.1f * MSize), MSize);
        }

        private void OnDrawGizmosSelected()
        {
            if (_mNavMesh)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(_mNavMesh.sourceBounds.center, _mNavMesh.sourceBounds.size);
            }

            Gizmos.color = Color.yellow;
            var bounds = QuantizedBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.color = Color.green;
            var center = MTracked ? MTracked.position : transform.position;
            Gizmos.DrawWireCube(center, MSize);
        }
    }
}
