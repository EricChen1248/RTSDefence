using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Tagging component for use with the LocalNavMeshBuilder
// Supports mesh-filter and terrain - can be extended to physics and/or primitives
namespace External_Libraries.NavMeshComponents.Scripts
{
    [DefaultExecutionOrder(-200)]
    public class NavMeshSourceTag : MonoBehaviour
    {
        // Global containers for all active mesh/terrain tags
        public static List<MeshFilter> MMeshes = new List<MeshFilter>();
        public static List<Terrain> MTerrains = new List<Terrain>();

        private void OnEnable()
        {
            var m = GetComponent<MeshFilter>();
            if (m != null)
            {
                MMeshes.Add(m);
            }

            var t = GetComponent<Terrain>();
            if (t != null)
            {
                MTerrains.Add(t);
            }
        }

        private void OnDisable()
        {
            var m = GetComponent<MeshFilter>();
            if (m != null)
            {
                MMeshes.Remove(m);
            }

            var t = GetComponent<Terrain>();
            if (t != null)
            {
                MTerrains.Remove(t);
            }
        }

        // Collect all the navmesh build sources for enabled objects tagged by this component
        public static void Collect(ref List<NavMeshBuildSource> sources)
        {
            sources.Clear();

            foreach (var mf in MMeshes)
            {
                if (mf == null) continue;

                var m = mf.sharedMesh;
                if (m == null) continue;

                var s = new NavMeshBuildSource
                {
                    shape = NavMeshBuildSourceShape.Mesh,
                    sourceObject = m,
                    transform = mf.transform.localToWorldMatrix,
                    area = 0
                };
                sources.Add(s);
            }

            foreach (var t in MTerrains)
            {
                if (t == null) continue;

                var s = new NavMeshBuildSource
                {
                    shape = NavMeshBuildSourceShape.Terrain,
                    sourceObject = t.terrainData,
                    transform = Matrix4x4.TRS(t.transform.position, Quaternion.identity, Vector3.one),
                    area = 0
                };
                // Terrain system only supports translation - so we pass translation only to back-end
                sources.Add(s);
            }
        }
    }
}
