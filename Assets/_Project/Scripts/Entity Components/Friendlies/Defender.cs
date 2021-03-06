﻿using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Entity_Components.Friendlies
{
    public class Defender : PlayerComponent
    {
        public IEnumerator CheckingCollision;

        public IEnumerator CheckingDistance;
        public int Damage;
        public float Radius;
        public float ReloadTime;

        public override void MoveToLocation(Vector3 target)
        {
            if (CheckingDistance != null) StopCoroutine(CheckingDistance);
            if (CheckingCollision != null) StopCoroutine(CheckingCollision);

            Agent.SetDestination(target);
            CheckingDistance = CheckDistanceRoutine();
            StartCoroutine(CheckingDistance);

            Agent.isStopped = false;
        }

        protected virtual IEnumerator CheckCollision()
        {
            while (true)
            {
                var colliders = Physics.OverlapSphere(transform.position, Radius,
                    RaycastHelper.LayerMaskDictionary["Enemies"]);

                if (colliders.Length > 0)
                {
                    StartCoroutine(Attack(colliders[0].gameObject));
                    yield break;
                }

                for (var i = 0; i < 10; i++) yield return new WaitForFixedUpdate();
            }
        }

        protected virtual IEnumerator Attack(GameObject go)
        {
            var rotate = RotateToTarget(go);
            StartCoroutine(rotate);

            var health = go.GetComponent<HealthComponent>();
            Animator.SetBool("Attacking", true);
            var targetCollider = go.GetComponent<Collider>();
            var radius = Radius * Radius;

            while (health.Health > 0 && health != null)
            {
                yield return new WaitForSeconds(ReloadTime);

                // If target no longer in range
                var colliders = Physics.OverlapSphere(transform.position, radius,
                    RaycastHelper.LayerMaskDictionary["Enemies"]);
                if (!colliders.Contains(targetCollider)) break;
                health.Damage(Damage);
            }

            StopCoroutine(rotate);
            StartCoroutine(CheckCollision());

            Animator.SetBool("Attacking", false);
            // Wait for animation to stop
            yield return new WaitForSeconds(1);

            Agent.isStopped = false;
        }

        protected IEnumerator RotateToTarget(GameObject go)
        {
            while (go != null)
            {
                var look = go.transform.position - transform.position;
                var newDir = Vector3.RotateTowards(transform.forward, look, Time.deltaTime * 10, 10);

                transform.rotation = Quaternion.LookRotation(newDir);

                yield return new WaitForFixedUpdate();
            }
        }


        protected IEnumerator CheckDistanceRoutine()
        {
            var destination = Agent.destination;
            while (true)
            {
                if ((transform.position - destination).sqrMagnitude < 3f) break;
                yield return new WaitForFixedUpdate();
            }

            CheckingDistance = null;
            CheckingCollision = CheckCollision();
            StartCoroutine(CheckingCollision);
        }
    }
}