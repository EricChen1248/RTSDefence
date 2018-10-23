using Scripts.Graphic_Components;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class HealthComponent : MonoBehaviour
    {
        public delegate void DeathEventHandler(HealthComponent sender);

        private HealthBarComponent _healthBarComponent;

        public event DeathEventHandler OnDeath;
        public int MaxHealth = 100;
        public int Health = 100;

        public void Start()
        {
            _healthBarComponent = GetComponentInChildren<HealthBarComponent>();
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
            OnDeath?.Invoke(this);
        }

        private void ReportHealth()
        {
            _healthBarComponent.ReportProgress((float)Health / MaxHealth);
        }
    }
}
