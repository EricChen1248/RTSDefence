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
        public Animation _animation;

        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            _enemyComponent = GetComponent<EnemyComponent>();
            _animation = GetComponentInChildren<Animation>();
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
            print("starting routine");
            StartCoroutine(RotateToTarget());
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            var health = _tempTarget.GetComponent<HealthComponent>();
            float radius;
            try
            {
                radius = _tempTarget.GetComponent<SphereCollider>().radius;
            }
            catch (MissingComponentException)
            {
                radius = _tempTarget.GetComponent<CapsuleCollider>().radius;
            }
            while (health.Health > 0)
            {
                if ((_tempTarget.transform.position - transform.position).sqrMagnitude >=
                    Math.Pow(_enemyComponent.Radius + 2 + radius, 2))
                {
                    print("stopping attack");
                    _tempTarget = null;
                    health.OnDeath -= OnTargetDeath;
                    Agent.isStopped = false;
                    break;
                }

                // Attack
                health.Damage(10);
                
                _animation.Play();

                yield return new WaitForSeconds(ReloadTime);
            }
        }

        private IEnumerator RotateToTarget()
        {
            var look = _tempTarget.transform.position - transform.position;
            while(true)
            {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, look, Time.deltaTime, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);
                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTargetDeath(HealthComponent target)
        {
            print(GetInstanceID());
        }
    }
}