using System.Collections;
using UnityEngine;

namespace Scripts.Entity_Components.Status
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealComponent : MonoBehaviour
    {
        public int Duration;
        public int Heal;


        // Use this for initialization
        void Start () {
		
        }

        private IEnumerator HealhRoutine()
        {
            var health = GetComponent<HealthComponent>();
            var i = 0;
            while (i < Duration)
            {
                i++;
                yield return new WaitForSeconds(1);
                health.Damage(-Heal);
            }

            Destroy(this);
        }
    }
}
