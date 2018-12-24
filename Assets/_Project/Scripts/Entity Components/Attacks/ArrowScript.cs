using System;
using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.Entity_Components.Status;
using Scripts.Navigation;
using Scripts.Towers;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{

    public enum ArrowType { Regular, Fire, Thunder, Slow }
    public class ArrowScript : AmmoBase
    {
        public float Height = 1f;
        public float Acceleration = 0.0001f;
        public enum ArrowType { Regular, Fire, Explode }

        public ArrowType Type;
        private IEnumerator _currentCoroutine;

        public void Start()
        {

        }
	
        public override void Fire()
        {
            _currentCoroutine = FireRoutine(transform.position, Target.position);
            StartCoroutine(CheckCollision());
            StartCoroutine(_currentCoroutine);
        }
    
        private IEnumerator FireRoutine(Vector3 startPos, Vector3 endPos)
        {
            float maxHeight;

            if (startPos.y >= endPos.y)
            {
                maxHeight = startPos.y + Height;
            }
            else
            {
                maxHeight = endPos.y + Height;
            }
            var time = Mathf.Sqrt(2 * (maxHeight - startPos.y) / Acceleration) + Mathf.Sqrt(2 * (maxHeight - endPos.y) / Acceleration);

            var velocityV = Acceleration * Mathf.Sqrt(2 * (maxHeight - startPos.y) / Acceleration);

            var velocityH = new Vector3(endPos.x - startPos.x , 0 , endPos.z - startPos.z) / time;

            var lastPos = transform.position;
            while (transform.position.y > 0)
            {
                velocityV = velocityV - Acceleration;
                transform.position = transform.position + velocityH + velocityV * Vector3.up;
                transform.LookAt(2 * transform.position - lastPos);
                lastPos = transform.position;
                yield return new WaitForFixedUpdate();
            }
    
            // Explosion
            Destroy(gameObject);
        }

        private IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 0, 0.065f / 2),
                    transform.position - new Vector3(0, 0, 0.065f / 2), 0.03f, Layer);
                if (colliders.Length > 0)
                {
                    if (RaycastHelper.InLayer(Layer, colliders[0].gameObject.layer))
                    {
                        var health = colliders[0].GetComponent<HealthComponent>();
                        health.Damage(Damage);
                        switch (Type)
                        {
                            case ArrowType.Regular:
                                break;
                            case ArrowType.Fire:
                                colliders[0].gameObject.AddComponent<BurnComponent>();
                                break;
                                /*
                            case ArrowType.Slow:
                                colliders[0].gameObject.AddComponent<SlowComponent>();
                                break;
                            case ArrowType.Thunder:
                                colliders[0].gameObject.AddComponent<ThunderComponent>();
                                break;
                                */
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    Destroy(gameObject);
                }
                else if(transform.position.y < 0)
                {
                    Destroy(gameObject);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
