using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Buildable_Components
{
    public class EntranceWaiter : MonoBehaviour
    {
        public int WaitTime = 10;
        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(WaitAndEnter(other));
        }

        private IEnumerator WaitAndEnter(Component other)
        {
            var agent = other.GetComponentInParent<NavMeshAgent>();
            var waitPos = transform.position +
                    ((other.transform.position - transform.position + transform.forward).sqrMagnitude > (other.transform.position - transform.position - transform.forward).sqrMagnitude
                    ? transform.forward
                    : -transform.forward) * 0.9f;

            var originalDest = agent.destination;

            agent.destination = waitPos;
            while (agent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            for (var i = 0; i < WaitTime; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            if ((originalDest - transform.position).sqrMagnitude < 1.5f)
            {
                var otherSide = transform.position + (transform.position - waitPos) * 1.45f;
                agent.destination = otherSide;
            }
            else
            {
                agent.destination = originalDest;
            }
        }
    }
}
