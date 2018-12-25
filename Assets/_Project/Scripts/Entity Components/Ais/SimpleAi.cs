using System;
using System.Collections;
using System.Linq;
using Scripts.Buildable_Components;
using Scripts.Entity_Components.Misc;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleAi : SingularAiBase
    {
        protected Animator Animator;
        protected EnemyData Data;
        protected EnemyComponent EnemyComponent;

        public override void Start()
        {
            base.Start();

            Agent = GetComponent<NavMeshAgent>();
            EnemyComponent = GetComponent<EnemyComponent>();
            Animator = GetComponent<Animator>();
            StopTemp = false;
            Data = EnemyComponent.Data;
            StartCoroutine(CheckCollision());
        }

        public override void FindTarget()
        {
            TargetTo(Target, true);
            Animator.SetBool("Walking", true);
        }

        public override void StopTempTarget()
        {
            StopTemp = true;
        }


        protected virtual IEnumerator CheckCollision()
        {
            var colliders = new Collider[100];
            while (true)
            {
                var count = Physics.OverlapSphereNonAlloc(transform.position, Data.Radius, colliders, Data.TargetLayers);

                if (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        var collider = colliders[i];
                        var buildable = collider.gameObject.GetComponent<Buildable>();
                        if (buildable != null)
                        {
                            if (buildable.Data.Types.Contains(DefenceType.Wall))
                            {
                                if ((collider.transform.position - transform.position).sqrMagnitude > 1f)
                                {
                                    continue;
                                }
                            }
                        }

                        Agent.SetDestination(collider.transform.position);
                        Animator.SetBool("Walking", false);

                        TempTarget = collider.gameObject;
                        StopTemp = false;

                        StartCoroutine(Attack());
                        yield break;
                    }
                }

                for (var i = 0; i < 5; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private IEnumerator Attack()
        {
            var rotate = RotateToTarget();

            StartCoroutine(rotate);

            Animator.SetBool("Attacking", true);
            var health = TempTarget.GetComponent<HealthComponent>();
            var targetCollider = TempTarget.GetComponent<Collider>();

            var radius = Data.Radius;
            radius *= radius;

            while (health != null && health.Health > 0)
            {
                yield return new WaitForSeconds(ReloadTime);

                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius, Data.TargetLayers);
                if (!colliders.Contains(targetCollider)) break;

                health.Damage(Data.Damage);
            }
            yield return new WaitForFixedUpdate();

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetBool("Attacking", false);
            Animator.SetBool("Walking", true);

            yield return new WaitForSeconds(ReloadTime);
            TempTarget = null;
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.SetDestination(Target.transform.position);
        }

        protected IEnumerator RotateToTarget()
        {
            while (TempTarget != null)
            {
                var look = TempTarget.transform.position - transform.position;
                var newDir = Vector3.RotateTowards(transform.forward, look, Time.deltaTime, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);

                yield return new WaitForFixedUpdate();
            }
        }
    }
}