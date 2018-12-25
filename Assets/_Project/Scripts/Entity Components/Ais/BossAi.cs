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
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(6.5f);
            print("attacking");
            TargetTo(Target, true);
            Animator.SetBool("Walking", true);
            Agent.isStopped = false;
        }

        protected override IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Data.Radius, Data.TargetLayers);

                if (colliders.Length > 0)
                    foreach (var collider in colliders)
                    {
                        var buildable = collider.gameObject.GetComponent<Buildable>();
                        if (buildable != null && buildable.Data.Name == "Wall")
                            if ((collider.transform.position - transform.position).sqrMagnitude > 2)
                                continue;
                        Agent.isStopped = true;
                        Animator.SetBool("Walking", false);

                        TempTarget = collider.gameObject;
                        StopTemp = false;

                        StartCoroutine(Attack());
                        yield break;
                    }

                for (var i = 0; i < 10; i++) yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Attack()
        {
            var rotate = RotateToTarget();
            StartCoroutine(rotate);

            var health = TempTarget.GetComponent<HealthComponent>();
            health.OnDeath += OnTargetDeath;
            var targetCollider = TempTarget.GetComponent<Collider>();
            var radius = Data.Radius;
            radius *= radius;
            while (health.Health > 0 && health != null)
            {
                var atktype = Random.Range(3, 4);
                var colliders = Physics.OverlapSphere(transform.position, radius,
                    RaycastHelper.LayerMaskDictionary["Friendlies"]);
                var damage = 1;
                switch (atktype)
                {
                    case 1:

                        Animator.SetInteger("AttackType", 1);
                        yield return new WaitForSeconds(4f);
                        damage = 25;
                        break;

                    case 2:

                        Animator.SetInteger("AttackType", 2);
                        yield return new WaitForSeconds(4.5f);
                        damage = 30;
                        break;

                    case 3:

                        Animator.SetInteger("AttackType", 3);
                        yield return new WaitForSeconds(3.5f);
                        damage = 20;
                        break;
                }

                // If target no longer in range
                colliders = Physics.OverlapSphere(transform.position, radius,
                    RaycastHelper.LayerMaskDictionary["Friendlies"]);
                if (!colliders.Contains(targetCollider)) break;
                health.Damage(damage);

                Animator.SetInteger("AttackType", 0);
            }

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetInteger("AttackType", 0);
            Animator.SetBool("Walking", true);

            yield return new WaitForSeconds(0.5f);
            TempTarget = null;
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.isStopped = false;
        }

        private void OnTargetDeath(HealthComponent target)
        {
        }
    }
}