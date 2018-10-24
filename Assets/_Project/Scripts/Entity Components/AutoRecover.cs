using UnityEngine;

namespace Scripts.Entity_Components
{
    public class AutoRecover : MonoBehaviour
    {
        public int RecoverHealth;
        private HealthComponent _health;
        // Use this for initialization
        private void Start()
        {
            _health = GetComponent<HealthComponent>();
            _health.OnDeath += h =>
            {
                h.Health = RecoverHealth;
            };
        }
    }
}