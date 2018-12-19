using Scripts.Entity_Components.Misc;
using Scripts.Towers;
using UnityEngine;
using Scripts.Navigation;
using Scripts.Entity_Components.Attacks;
using System.Collections;
using System;

namespace Scripts.Entity_Components
{
    public class BulletScript : AmmoBase
    {
        public ArrowType Type;
        public int Damage = 1;

        public float Speed = 0.1f;

        public override void Fire()
        {
            StartCoroutine(CheckCollision());
            transform.rotation.SetLookRotation(Target.position - transform.position);
        }


        public void FixedUpdate()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);

            var rotation = Quaternion.LookRotation(Target.position - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);


            return;
        }

        IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 0, 0.065f / 2),
                    transform.position - new Vector3(0, 0, 0.065f / 2), 0.03f);
                if (colliders.Length > 0)
                {
                    if (RaycastHelper.InLayer(Layer, colliders[0].gameObject.layer))
                    {
                        var health = colliders[0].GetComponent<HealthComponent>();
                        health.Damage(5);
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
                    }

                    Destroy(gameObject);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
