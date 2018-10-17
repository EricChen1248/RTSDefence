using Graphic_Components;
using UnityEngine;

namespace Entity_Components
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
        }

        public void Damage(int damage)
        {
            Health -= damage;
            _healthBarComponent.ReportProgress(((float)Health)/MaxHealth);

            if (Health <= 0)
            {
                Death();
            }
        }


        private void Death()
        {
            // Kill of object;
            OnDeath?.Invoke(this);
        }
        
    }
}
