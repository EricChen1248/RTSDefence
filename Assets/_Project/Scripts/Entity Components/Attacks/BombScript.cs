using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{
    public class BombScript : MonoBehaviour
    {
        public int Damage = 20;
        public int Range = 5;
        public GameObject Smoke;
        
        public void FixedUpdate()
        {
            if (!(transform.position.y <= 0)) return;
            var colliders = Physics.OverlapSphere(transform.position, Range,
                RaycastHelper.LayerMaskDictionary["Enemies"]);

            foreach (var collider in colliders)
            {
                var health = collider.GetComponent<HealthComponent>();
                if (health != null)
                {
                    health.Damage(Damage);
                }
            }

            var smoke = Instantiate(Smoke);
            smoke.transform.position = transform.position;
            Destroy(this);
        }
    }
}