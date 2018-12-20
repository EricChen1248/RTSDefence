using Scripts.Entity_Components.Misc;
using UnityEngine;
using static Scripts.Entity_Components.Attacks.ArrowScript;

namespace Scripts.Entity_Components
{
    public class BulletScript : MonoBehaviour
    {
        public int Damage = 1;

        public float Speed = 1;

        public Transform Target;

        public ArrowType Type;

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

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Target.gameObject.layer)
            {
                other.GetComponentInParent<HealthComponent>().Damage(Damage);
                switch (Type)
                {
                    case ArrowType.Regular:
                        break;
                    case ArrowType.Fire:
                        break;
                    default:
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
