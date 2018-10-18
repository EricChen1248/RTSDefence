using System;
using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : AiBase
    {
        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
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
            Target = other.gameObject;
            var health = Target.GetComponent<HealthComponent>();
            health.OnDeath += OnTargetDeath;
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            var health = Target.GetComponent<HealthComponent>();
            while (health.Health > 0)
            {
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
