using System;
using System.Collections;
using Scripts.Entity_Components.Status;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Entity_Components.Attacks
{
    public class ArrowScript : MonoBehaviour {

        public Transform Target;
        public float Height = 20f;
        public float Acceleration = 0.01f;
        public enum ArrowType { Regular, Fire }

        public ArrowType Type;

        private IEnumerator _currentCoroutine;


        // Use this for initialization
        private void Start ()
        {
            Fire(transform.position, Target.position);
        }
	
        public void Fire(Vector3 startPos, Vector3 endPos)
        {
            _currentCoroutine = FireRoutine(startPos, endPos);

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

            for (var i = 0f; i <= Mathf.Ceil(time) + 5; i++)
            {
                velocityV = velocityV - Acceleration;
                transform.rotation = Quaternion.LookRotation(new Vector3(velocityH.x,velocityV,velocityH.z));
                transform.position = transform.position + velocityH + velocityV * Vector3.up;
                yield return new WaitForFixedUpdate();
            }
    
            // Explosion

            Pool.ReturnToPool("Arrow", gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var health = other.GetComponent<HealthComponent>();
                health.Damage(50);
                switch (Type)
                {
                    case ArrowType.Regular:
                        break;
                    case ArrowType.Fire:
                        other.gameObject.AddComponent<BurnComponent>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            Pool.ReturnToPool("Arrow", gameObject);
        }


    }
}
