using Entity_Components.Ais;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components
{
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(AiBase))]
    public class EnemyComponent : MonoBehaviour
    {
        private AiBase _ai;
        private NavMeshAgent _agent;

        public EnemyData Data;

        public float Radius;

        // Use this for initialization
        public void Start()
        {

            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = true;

            _ai = GetComponent<AiBase>();
            _ai.TargetingLayers = Data.TargetLayers;
            _ai.ReloadTime = Data.ReloadTime;

            Radius = Data.Radius;

            GetComponent<SphereCollider>().radius = Radius;
            _ai.FindTarget();
        }
    }
}
