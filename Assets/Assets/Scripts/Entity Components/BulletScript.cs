using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components
{
    public class BulletScript : MonoBehaviour
    {

        public int Damage = 1;

        public float Speed = 1;

        public Transform Target;

        // Update is called once per frame
        public void FixedUpdate ()
        {
            var moveDir = Target.position - transform.position;
            transform.position += Vector3.Normalize(moveDir) * Speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == transform.parent)
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                other.GetComponentInParent<HealthComponent>().Damage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
