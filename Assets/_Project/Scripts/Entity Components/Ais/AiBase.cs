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
    }
}
