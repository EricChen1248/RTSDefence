using System;
using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Entity_Components.Status;
using Scripts.Towers;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{
    public class BulletScript : AmmoBase
    {
        public float Speed = 0.1f;
        public ArrowType Type;

        public override void Fire()
        {
            StartCoroutine(CheckCollision());
        }


        public void FixedUpdate()
        {
            try
            {
                var rotation = Quaternion.LookRotation(Target.position + Vector3.up * 0.5f - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10 * Speed);
                transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            }
            catch (MissingReferenceException)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator CheckCollision()
        {
            var collider = Target.GetComponent<Collider>();
            while (true)
            {
                var colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 0, 0.065f / 2),
                    transform.position - new Vector3(0, 0, 0.065f / 2), 0.03f, Layer);
                if (colliders.Length > 0)
                {
                    if (colliders.Contains(collider))
                    {
                        var health = colliders[0].GetComponent<HealthComponent>();
                        health.Damage(Damage);
                        switch (Type)
                        {
                            case ArrowType.Regular:
                                break;
                            case ArrowType.Fire:
                                break;
                            case ArrowType.Thunder:
                                var thunder = GetComponent<ThunderComponent>();
                                thunder.Spread();
                                colliders[0].gameObject.AddComponent<SlowComponent>();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        Destroy(gameObject);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}