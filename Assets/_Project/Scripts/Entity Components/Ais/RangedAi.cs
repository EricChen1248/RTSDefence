using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using Scripts.Towers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    public class RangedAi : SimpleAi
    {
        public GameObject Ammo;


        public new void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            EnemyComponent = GetComponent<EnemyComponent>();
            Animator = GetComponent<Animator>();
            StopTemp = false;
            Data = EnemyComponent.Data;
            StartCoroutine(CheckCollision());

            Ammo.transform.position = Vector3.up + Vector3.forward;
        }


        protected override IEnumerator CheckCollision()
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

                    StartCoroutine(RangedAttack());
                    yield break;
                }

                for (var i = 0; i < 10; i++) yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator RangedAttack()
        {
            var rotate = RotateToTarget();
            StartCoroutine(rotate);

            var health = TempTarget.GetComponent<HealthComponent>();
            Animator.SetBool("Attacking", true);
            var targetCollider = TempTarget.GetComponent<Collider>();
            var radius = Data.Radius;
            radius *= radius;

            while (health.Health > 0 && health != null)
            {
                yield return new WaitForSeconds(ReloadTime);
                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius,
                    RaycastHelper.LayerMaskDictionary["Friendlies"]);
                if (!colliders.Contains(targetCollider)) break;

                var ammo = Instantiate(Ammo, transform);
                //ammo.transform.position = transform.position;
                var script = ammo.GetComponent<AmmoBase>();
                script.Layer = RaycastHelper.LayerMaskDictionary["Friendlies"];
                script.Target = targetCollider.transform;
                script.Fire();
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
    }
}