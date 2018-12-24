using Scripts.Entity_Components.Misc;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class AutoRecover : MonoBehaviour
    {
        private HealthComponent _health;

        public int RecoverHealth;

        // Use this for initialization
        private void Start()
        {
            _health = GetComponent<HealthComponent>();
            _health.OnDeath += h => { h.Health = RecoverHealth; };
        }
    }
}