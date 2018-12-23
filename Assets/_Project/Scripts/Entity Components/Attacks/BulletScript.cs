using System;
using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.Towers;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{
    public class BulletScript : AmmoBase
    {
        public ArrowType Type;

        public float Speed = 0.1f;

        public override void Fire()
        {
            StartCoroutine(CheckCollision());
        }


        public void FixedUpdate()
        {
            var rotation = Quaternion.LookRotation(Target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }

        private IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 0, 0.065f / 2),
                    transform.position - new Vector3(0, 0, 0.065f / 2), 0.03f, Layer);
                if (colliders.Length > 0)
                {
                    var health = colliders[0].GetComponent<HealthComponent>();
                    health.Damage(Damage);
                    switch (Type)
                    {
                        case ArrowType.Regular:
                            break;
                        case ArrowType.Fire:
                            //colliders[0].gameObject.AddComponent<BurnComponent>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    Destroy(gameObject);
                }
                yield return new WaitForFixedUpdate();

            }
        }
    }
}
