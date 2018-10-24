using System;
using System.Collections;
using Scripts.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : AiBase
    {
        private EnemyComponent _enemyComponent;
        private GameObject _tempTarget;

        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            _enemyComponent = GetComponent<EnemyComponent>();
        }

        public override void FindTarget()
        {
            Target = CoreController.Instance.CoreGameObject;

            Agent.destination = Target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!InLayerMask(TargetingLayers, other.gameObject.layer)) return;
            Agent.isStopped = true;
            _tempTarget = other.gameObject;
            var health = _tempTarget.GetComponent<HealthComponent>();
            health.OnDeath += OnTargetDeath;
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            var health = _tempTarget.GetComponent<HealthComponent>();
            while (health.Health > 0)
            {
                print((_tempTarget.transform.position - transform.position).sqrMagnitude);
                if ((_tempTarget.transform.position - transform.position).sqrMagnitude >=
                    Math.Pow(_enemyComponent.Radius + 1, 2))
                {
                    _tempTarget = null;
                    health.OnDeath -= OnTargetDeath;
                    Agent.isStopped = false;
                    break;
                }

                // Attack
                health.Damage(1);

                yield return new WaitForSeconds(ReloadTime);
            }
        }

        private void OnTargetDeath(HealthComponent target)
        {
            print(GetInstanceID());
        }
    }
}