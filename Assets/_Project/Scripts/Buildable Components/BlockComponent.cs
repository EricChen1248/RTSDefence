using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Buildable_Components
{
    [DefaultExecutionOrder(1)]
    public class BlockComponent : MonoBehaviour
    {
        // Use this for initialization
        public void Start()
        {
            GetComponent<HealthComponent>().OnDeath += BlockComponent_OnDeath;
        }

        private void BlockComponent_OnDeath(HealthComponent sender)
        {
            var overlaps = Physics.OverlapBox(transform.position, new Vector3(1, 2, 1));

            foreach (var obj in overlaps)
            {
                var health = obj.GetComponentInChildren<HealthComponent>();
                if (health != null) health.Damage(health.Health);
            }
        }
    }
}