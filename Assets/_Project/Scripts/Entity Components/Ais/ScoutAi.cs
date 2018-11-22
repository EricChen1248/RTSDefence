using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    public class ScoutAi : AiBase
    {
        private int _previousDirection;
        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            _previousDirection = -1;
        }

		public override void FindTarget()
		{
		    var target = CoreController.Instance.CoreGameObject.transform;

		    var direction = target.position - transform.position;

		    var magnitude = direction.magnitude;

            // Make scout biased into keep moving the current direction
		    var dir = _previousDirection == -1 ? Random.Range(0, 2) : Random.Range(0,5);
		    if (_previousDirection == -1) _previousDirection = 0;

            var cross = Vector3.Cross(dir <= _previousDirection ? Vector3.up : Vector3.down, direction).normalized *
		                Random.Range(magnitude / 2, magnitude / 3 * 2);

		    _previousDirection = dir <= _previousDirection ? 3 : 0;

		    Agent.destination = transform.position + direction / 2 + cross;
		    StartCoroutine(CheckDistance());
		}

        private IEnumerator CheckDistance()
        {
            var lr = GetComponent<LineRenderer>();
            while (true)
            {
                var distanceToTarget = (transform.position - Agent.destination).sqrMagnitude;
                if (distanceToTarget < 2f) break;




                yield return new WaitForFixedUpdate();
            }

            print("Destination reached");
            FindTarget();
        }

        void Update()
        {
        }

    }
}