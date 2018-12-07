using System;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AiBase : MonoBehaviour
    {
        public GameObject Target;
        public float ReloadTime;
        protected NavMeshAgent Agent;

        [HideInInspector]
        public LayerMask TargetingLayers;

        public abstract void FindTarget();
        public virtual bool TargetTo(GameObject obj, bool force){
            if (!InLayerMask(TargetingLayers, obj.layer) && !force){
                return false;
            }
            Target = obj;
            Agent.destination = Target.transform.position;
            return true;
        }

        protected static bool InLayerMask(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        protected void LeaveMap()
        {
            var x = 125 - Math.Abs(transform.position.x);
            var z = 125 - Math.Abs(transform.position.z);
            Agent.destination = x > z ? new Vector3(transform.position.x > 0 ? 125 : -125, 0, transform.position.z) : new Vector3(transform.position.x, 0, transform.position.z > 0 ? 125 : -125);
        }
    }
}
