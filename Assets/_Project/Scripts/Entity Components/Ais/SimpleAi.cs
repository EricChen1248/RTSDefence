using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : SingularAiBase
    {
        protected EnemyComponent EnemyComponent;
        protected EnemyData Data;
        protected Animator Animator;

        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            EnemyComponent = GetComponent<EnemyComponent>();
            Animator = GetComponent<Animator>();
            StopTemp = false;
            Data = EnemyComponent.Data;
            StartCoroutine(CheckCollision());
        }

        public override void FindTarget()
        {
            TargetTo(Target, force: true);

            Animator.SetBool("Walking", true);
        }

        public override void StopTempTarget()
        {
            StopTemp = true;
        }


        private IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Data.Radius, Data.TargetLayers);

                if (colliders.Length > 0)
                {
                    Agent.isStopped = true;
                    Animator.SetBool("Walking", false);

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
            Animator.SetBool("Attacking", true);
            var targetCollider = TempTarget.GetComponent<Collider>();
            var radius = Data.Radius;
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
                health.Damage(Data.Damage);
            }
            
            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetBool("Attacking", false);
            Animator.SetBool("Walking", true);

            yield return new WaitForSeconds(ReloadTime);
            TempTarget = null;
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.isStopped = false;
        }

        protected IEnumerator RotateToTarget()
        {
            while(true)
            {
                var look = TempTarget.transform.position - transform.position;
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