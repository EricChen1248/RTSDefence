using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class BulletScript : MonoBehaviour
    {
        public int Damage = 1;

        public float Speed = 1;

        public Transform Target;

        // Update is called once per frame

        public void Start()
        {
            var bulletParent = GameObject.Find("BulletCollection");
            if (bulletParent == null)
            {
                bulletParent = Instantiate(new GameObject());
                bulletParent.name = "BulletCollection";
            }

            transform.parent = bulletParent.transform;
        }

        public void FixedUpdate ()
        {
            if (! Target.gameObject.activeSelf) 
            {
                Destroy(gameObject);    
            }
            var moveDir = Target.position - transform.position;
            transform.position += Vector3.Normalize(moveDir) * Speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Target.gameObject.layer)
            {
                other.GetComponentInParent<HealthComponent>().Damage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
