using System.Collections;
using Scripts.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    public class ScoutAi : AiBase
    {
        private Animator _animator;

        private int _count;
        private int _previousDirection;

        public override void Start()
        {
            base.Start();
            Agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _previousDirection = -1;
            _animator.SetBool("Walking", true);
        }

        public override void FindTarget()
        {
            var target = CoreController.Instance.CoreGameObject.transform;

            var direction = target.position - transform.position;

            var magnitude = direction.magnitude;

            // Make scout biased into keep moving the current direction
            var dir = _previousDirection == -1 ? Random.Range(0, 2) : Random.Range(0, 5);
            if (_previousDirection == -1) _previousDirection = 0;

            var cross = Vector3.Cross(dir <= _previousDirection ? Vector3.up : Vector3.down, direction).normalized *
                        Random.Range(magnitude / 2, magnitude / 3 * 2);

            _previousDirection = dir <= _previousDirection ? 3 : 0;

            Agent.destination = transform.position + direction / 2 + cross;
            StartCoroutine(CheckDistance());
        }

        private IEnumerator CheckDistance()
        {
            var distanceToTarget = 3f;
            while (distanceToTarget > 2f)
            {
                yield return new WaitForFixedUpdate();
                distanceToTarget = (transform.position - Agent.destination).sqrMagnitude;
            }

            if (_count++ < 10)
            {
                FindTarget();
            }
            else
            {
                LeaveMap();

                distanceToTarget = 3f;
                while (distanceToTarget > 2f)
                {
                    yield return new WaitForFixedUpdate();
                    distanceToTarget = (transform.position - Agent.destination).sqrMagnitude;
                }

                WaveController.Instance.AddScore(100);
                Destroy(gameObject);
            }
        }
    }
}