using System.Collections;
using System.Linq;
using Scripts.Buildable_Components;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class BossAi : SimpleAi
    {
        public override void Start()
        {
            base.Start();
            StartCoroutine(Spawn());
            Agent.isStopped = true;
            var health = GetComponent<HealthComponent>();
            health.MaxHealth = 5000;
            health.Health = health.MaxHealth;
            health.OnDeath += Health_OnDeath;
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(6.5f);
            TargetTo(Target, true);

            Animator.SetBool("Walking", true);
            Agent.isStopped = false;
        }

        protected override IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Data.Radius, RaycastHelper.LayerMaskDictionary["Friendlies"]);

                if (colliders.Length > 0)
                {
                    foreach (var collider in colliders)
                    {
                        var buildable = collider.gameObject.GetComponent<Buildable>();
                        if (buildable != null && buildable.Data.Name == "Wall")
                        {
                            if ((collider.transform.position - transform.position).sqrMagnitude > 2)
                                continue;
                        }

                        Agent.isStopped = true;
                        Animator.SetBool("Walking", false);
                        TempTarget = collider.gameObject;
                        StartCoroutine(Attack());
                        yield break;
                    }
                }

                for (var i = 0; i < 5; i++) yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Attack()
        {
            var rotate = RotateToTarget();
            StartCoroutine(rotate);

            var health = TempTarget.GetComponent<HealthComponent>();
            var targetCollider = TempTarget.GetComponent<Collider>();
            var radius = Data.Radius * Data.Radius;
            while (health != null && health.Health > 0)
            {
                var atktype = Random.Range(1, 4);
                var damage = 1;
                switch (atktype)
                {
                    case 1:
                        Animator.SetInteger("AttackType", 1);
                        yield return new WaitForSeconds(1.96f);
                        damage = 100;
                        break;
                    case 2:
                        Animator.SetInteger("AttackType", 2);
                        yield return new WaitForSeconds(2f);
                        damage = 120;
                        break;
                    case 3:
                        Animator.SetInteger("AttackType", 3);
                        yield return new WaitForSeconds(2f);
                        damage = 80;
                        break;
                    default:
                        break;
                }

                health.Damage(damage);

                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius, RaycastHelper.LayerMaskDictionary["Friendlies"]);
                if (!colliders.Contains(targetCollider)) break;

                Animator.SetInteger("AttackType", 0);
            }

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetInteger("AttackType", 0);

            yield return new WaitForSeconds(1);

            Animator.SetBool("Walking", true);
            Agent.isStopped = false;
        }

        private void Health_OnDeath(HealthComponent sender)
        {
            Animator.SetInteger("AttackType", 0);
            Animator.SetBool("Death", true);
        }
    }
}