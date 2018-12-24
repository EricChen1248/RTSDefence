using Scripts.Graphic_Components;
using UnityEngine;

namespace Scripts.Entity_Components.Misc
{
    [DefaultExecutionOrder(-100)]
    public class HealthComponent : MonoBehaviour
    {
        public delegate void DeathEventHandler(HealthComponent sender);

        private int _maxHealth = 100;
        public int Health = 100;

        public HealthBarComponent HealthBarComponent;

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                Health = value;
                ReportHealth();
            }
        }

        public event DeathEventHandler OnDeath;

        public void Start()
        {
            HealthBarComponent = GetComponentInChildren<HealthBarComponent>(true);
            ReportHealth();
        }

        public void Damage(int damage)
        {
            Health -= damage;
            ReportHealth();
            if (Health <= 0) Death();
        }

        private void Death()
        {
            // Kill off object;

            foreach (var coll in GetComponentsInParent<Collider>()) coll.enabled = false;
            OnDeath?.Invoke(this);
        }

        public void ReportHealth()
        {
            var percent = (float) Health / MaxHealth;
            percent = float.IsNaN(percent) ? 0 : percent;
            HealthBarComponent.ReportProgress(percent);
        }
    }
}