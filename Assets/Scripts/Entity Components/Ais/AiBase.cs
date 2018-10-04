using System.Xml.Schema;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Ais
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AiBase : MonoBehaviour
    {
        public GameObject Target;
        public float ReloadTime;
        protected NavMeshAgent Agent;

        [HideInInspector]
        public LayerMask TargetingLayers;

        public abstract void FindTarget();


        protected static bool InLayerMask(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}
