using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.GUI;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Friendlies
{
    public class Bomber : Defender
    {
        public override void Start()
        {
            base.Start();
            _type = "Bomber";
        }

        protected override IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Radius,
                    RaycastHelper.LayerMaskDictionary["Enemies"]);

                if (colliders.Length > 0)
                {
                    var omg = ObjectMenuGroupComponent.Instance;
                    omg.SetButton(0, "Explode", () => StartCoroutine(Attack(null)));
                    omg.Show();
                    // TODO : omg.SetButton
                    yield break;
                }

                for (var i = 0; i < 10; i++) yield return new WaitForFixedUpdate();
            }
        }

        protected override IEnumerator Attack(GameObject go)
        {
            Agent.isStopped = true;
            Animator.SetBool("Attacking", true);
            yield return new WaitForSeconds(ReloadTime);

            // If target no longer in range
            var colliders =
                Physics.OverlapSphere(transform.position, Radius, RaycastHelper.LayerMaskDictionary["Enemies"]);
            foreach (var collider in colliders)
            {
                var health = collider.GetComponent<HealthComponent>();
                health.Damage(Damage);
            }

            // Instantiate Bomb

            Destroy(gameObject);
        }
    }
}