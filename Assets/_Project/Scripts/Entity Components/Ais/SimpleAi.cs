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

        private bool _stopAttack;
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
            _stopAttack = false;
            StartCoroutine(Attack());
        }

        private void OnTriggerExit(Collider other)
        {
            if (_stopAttack) return;
            _stopAttack = (other.gameObject == _tempTarget);
        }

        private IEnumerator Attack()
        {
            var health = _tempTarget.GetComponent<HealthComponent>();
            while (health.Health > 0 && !_stopAttack)
            {
                // Attack
                health.Damage(1);

                yield return new WaitForSeconds(ReloadTime);
            }

            health.OnDeath -= OnTargetDeath;

            Agent.isStopped = false;

            _stopAttack = true;
            _tempTarget = null;
        }

        private void OnTargetDeath(HealthComponent target)
        {
            print(GetInstanceID());
        }
    }
}