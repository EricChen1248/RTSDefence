using UnityEngine;
using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using System.Linq;
using Scripts.Towers;

namespace Scripts.Entity_Components.Friendlies
{
    public class RangeDefender : Defender
    {
        public GameObject Ammo;

        protected override IEnumerator Attack(GameObject go)
        {
            var rotate = RotateToTarget(go);
            StartCoroutine(rotate);

            var health = go.GetComponent<HealthComponent>();
            Animator.SetBool("Attacking", true);
            var targetCollider = go.GetComponent<Collider>();
            var radius = Radius * Radius;

            while (health.Health > 0 && health != null)
            {
                yield return new WaitForSeconds(ReloadTime);

                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius, RaycastHelper.LayerMaskDictionary["Enemies"]);
                if (!colliders.Contains(targetCollider))
                {
                    break;
                }

                var ammo = Instantiate(Ammo, transform);
                var script = ammo.GetComponent<AmmoBase>();
                ammo.transform.localPosition = new Vector3(-1, 1.5f, 0);
                script.Layer = RaycastHelper.LayerMaskDictionary["Enemies"];
                script.Damage = 2;
                script.Parent = transform.root;
                script.Target = go.transform;
                script.Fire();
            }
            

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetBool("Attacking", false);
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.isStopped = false;
        }
    }
}
