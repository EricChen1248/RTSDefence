using Scripts.Graphic_Components;
using UnityEngine;

namespace Scripts.Entity_Components.Misc
{

    [DefaultExecutionOrder(0)]
    public class HealthComponent : MonoBehaviour
    {
        public delegate void DeathEventHandler(HealthComponent sender);

        public HealthBarComponent HealthBarComponent;

        public event DeathEventHandler OnDeath;
        public int MaxHealth = 100;
        public int Health = 100;

        public void Start()
        {
            HealthBarComponent = GetComponentInChildren<HealthBarComponent>(includeInactive: true);
            Health = MaxHealth;

            ReportHealth();
        }

        public void Damage(int damage)
        {
            Health -= damage;
            ReportHealth();
            if (Health <= 0)
            {
                Death();
            }
        }
        
        private void Death()
        {
            // Kill off object;

            foreach (var coll in GetComponentsInParent<Collider>())
            {
                coll.enabled = false;
            }
            OnDeath?.Invoke(this);
        }

        private void ReportHealth()
        {
            HealthBarComponent.ReportProgress((float)Health / MaxHealth);
        }
    }
}
