using Scripts.Entity_Components.Ais;
using Scripts.Entity_Components.Misc;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components
{
    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(AiBase))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(SingularAiBase))]
    public class EnemyComponent : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private AiBase _ai;
        private GroupFinder _gf;

        public EnemyData Data;

        public float Radius;

        // Use this for initialization
        public void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = Data.MovementSpeed;
            _agent.enabled = true;

            _ai = GetComponent<AiBase>();
            _ai.TargetingLayers = Data.TargetLayers;
            _ai.ReloadTime = Data.ReloadTime;

            if (Data.FindGroup)
            {
                _gf = GetComponent<GroupFinder>();
                _gf.enabled = true;
            }

            Radius = Data.Radius;

            //_ai.FindTarget();
        }
    }
}