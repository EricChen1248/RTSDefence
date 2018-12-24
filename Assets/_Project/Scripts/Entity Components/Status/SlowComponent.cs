using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Status
{
    public class SlowComponent : MonoBehaviour
    {
        public int Duration = 5;

        public float Slow = 2.0f;

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(SlowDown());
        }

        private IEnumerator SlowDown()
        {
            var agent = GetComponent<NavMeshAgent>();
            agent.speed -= Slow;
            yield return new WaitForSeconds(Duration);
            agent.speed += Slow;
            Destroy(this);
        }
    }
}