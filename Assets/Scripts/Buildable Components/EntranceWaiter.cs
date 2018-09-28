using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Buildable_Components
{
    public class EntranceWaiter : MonoBehaviour
    {
        public int WaitTime = 10;
        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(WaitAndEnter(other));
        }

        private IEnumerator WaitAndEnter(Collider other)
        {
            var agent = other.GetComponentInParent<NavMeshAgent>();
            var endPos = transform.position +
                    ((other.transform.position - transform.position + transform.forward).sqrMagnitude > (other.transform.position - transform.position - transform.forward).sqrMagnitude
                    ? transform.forward
                    : -transform.forward);

            var originalDest = agent.destination;

            agent.destination = endPos;
            while (agent.remainingDistance > 0)
            {
                yield return null;
            }

            for (var i = 0; i < WaitTime; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            agent.destination = originalDest;

        }
    }
}
