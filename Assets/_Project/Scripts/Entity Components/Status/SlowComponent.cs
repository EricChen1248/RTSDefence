using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.Entity_Components.Friendlies;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Status
{
    public class SlowComponent : MonoBehaviour
    {
        public float Slow = 2.0f;

        public int Duration = 5;
        // Use this for initialization
        private void Start ()
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
