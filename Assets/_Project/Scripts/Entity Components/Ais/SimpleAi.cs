using System;
using System.Collections;
using Scripts.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : SingularAiBase
    {
        private EnemyComponent _enemyComponent;
        public Animation Animation;

        private bool _stopAttack;
        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            _enemyComponent = GetComponent<EnemyComponent>();
            Animation = GetComponentInChildren<Animation>();
            StopTemp = false;
        }

        public override void FindTarget()
        {
            TargetTo(CoreController.Instance.CoreGameObject, force: true);
        }

        public override void StopTempTarget(){
            StopTemp = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!InLayerMask(TargetingLayers, other.gameObject.layer)) return;
            Agent.isStopped = true;
            TempTarget = other.gameObject;
            StopTemp = false;
            var health = TempTarget.GetComponent<HealthComponent>();
            health.OnDeath += OnTargetDeath;
            print("starting routine rotate to target");
            StartCoroutine(RotateToTarget());
            StartCoroutine(Attack());
        }

        private void OnTriggerExit(Collider other)
        {
            if (_stopAttack) return;
            _stopAttack = (other.gameObject == TempTarget);
        }

        private IEnumerator Attack()
        {
            var health = TempTarget.GetComponent<HealthComponent>();
            float radius;
            try
            {
                radius = TempTarget.GetComponent<SphereCollider>().radius;
            }
            catch (MissingComponentException)
            {
                radius = TempTarget.GetComponent<CapsuleCollider>().radius;
            }
            while (health.Health > 0)
            {
                if ((TempTarget.transform.position - transform.position).sqrMagnitude >=
                    Math.Pow(_enemyComponent.Radius + 2 + radius, 2))
                {
                    print("stopping attack");
                    TempTarget = null;
                    StopTemp = false;
                    health.OnDeath -= OnTargetDeath;
                    Agent.isStopped = false;
                    break;
                }

                // Attack
                health.Damage(10);
                
                Animation.Play();

                yield return new WaitForSeconds(ReloadTime);
            }

            health.OnDeath -= OnTargetDeath;

            Agent.isStopped = false;

            _stopAttack = true;
            TempTarget = null;
        }

        private IEnumerator RotateToTarget()
        {
            var look = TempTarget.transform.position - transform.position;
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