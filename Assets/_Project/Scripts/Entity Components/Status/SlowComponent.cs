using System.Collections;
using Scripts.Entity_Components.Misc;
using Scripts.Entity_Components.Friendlies;
using UnityEngine;

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
            var player = GetComponent<PlayerComponent>();
            player.Speed -= Slow;
            yield return new WaitForSeconds(Duration);
            player.Speed += Slow;
            Destroy(this);
        }
    }
}
