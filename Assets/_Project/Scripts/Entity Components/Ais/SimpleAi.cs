using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : SingularAiBase
    {
        private EnemyComponent _enemyComponent;
        private Animator _animator;

        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            _enemyComponent = GetComponent<EnemyComponent>();
            _animator = GetComponent<Animator>();
            StopTemp = false;

            StartCoroutine(CheckCollision());
        }

        public override void FindTarget()
        {
            TargetTo(Target, force: true);

            _animator.SetBool("Walking", true);
        }

        public override void StopTempTarget()
        {
            StopTemp = true;
        }


        private IEnumerator CheckCollision()
        {
            var radius = _enemyComponent.Radius;
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, radius, RaycastHelper.LayerMaskDictionary["Friendlies"]);

                if (colliders.Length > 0)
                {
                    Agent.isStopped = true;
                    _animator.SetBool("Walking", false);

                    TempTarget = colliders[0].gameObject;
                    StopTemp = false;

                    StartCoroutine(Attack());
                    yield break;
                }

                for (var i = 0; i < 10; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private IEnumerator Attack()
        {

            var rotate = RotateToTarget();
            StartCoroutine(rotate);

            var health = TempTarget.GetComponent<HealthComponent>();
            health.OnDeath += OnTargetDeath;
            _animator.SetBool("Attacking", true);
            var targetCollider = TempTarget.GetComponent<Collider>();
            var radius = _enemyComponent.Radius;
            radius *= radius;
            while (health.Health > 0 && health != null)
            {
                yield return new WaitForSeconds(ReloadTime);

                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius, RaycastHelper.LayerMaskDictionary["Friendlies"]);
                if (!colliders.Contains(targetCollider))
                {
                    break;
                }
                health.Damage(10);
            }

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            _animator.SetBool("Attacking", false);
            _animator.SetBool("Walking", true);

            TempTarget = null;
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.isStopped = false;
        }

        private IEnumerator RotateToTarget()
        {
            var look = TempTarget.transform.position - transform.position;
            while(true)
            {
                var newDir = Vector3.RotateTowards(transform.forward, look, Time.deltaTime, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTargetDeath(HealthComponent target)
        {

        }
    }
}