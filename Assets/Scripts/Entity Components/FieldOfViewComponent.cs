using UnityEngine;

namespace Entity_Components
{
    public class FieldOfViewComponent : MonoBehaviour
    {
        public float ViewRadius;
        [Range(0, 360)]
        public float ViewAngle;

        public LayerMask TargetMask;

        public Collider FindClosestTarget()
        {
            var targets = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);
            var closest = float.MaxValue;
            Collider closestTarget = null;
            foreach (var target in targets)
            {
                var t = target.transform;
                var dirToTarget = Vector3.ProjectOnPlane(t.position - transform.position, Vector3.up);

                if (closest <= dirToTarget.sqrMagnitude) continue;

                var ray = new Ray(transform.position, target.transform.position - transform.position);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit)) continue;
                if (!InLayerMask(TargetMask, hit.transform.gameObject.layer)) continue;

                closestTarget = target;
                closest = dirToTarget.sqrMagnitude;
            }

            return closestTarget;
        }

        public bool TargetInField(Vector3 target)
        {
            var dirToTarget = Vector3.ProjectOnPlane(target - transform.position, Vector3.up);
            return (Vector3.Angle(transform.forward, dirToTarget.normalized) < ViewAngle / 2);
        }

        private static bool InLayerMask(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}
