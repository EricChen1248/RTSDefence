using UnityEngine;

namespace Entity_Components
{
    public class HealthComponent : MonoBehaviour
    {
        public delegate void DeathEventHandler(HealthComponent sender);

        public event DeathEventHandler OnDeath;
        public int Health = 100;
        public void Damage(int damage)
        {
            Health -= damage;

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
