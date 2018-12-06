using Scripts.Entity_Components.Misc;
using Scripts.Graphic_Components;
using Scripts.Scriptable_Objects;
using UnityEngine;

namespace Scripts.Buildable_Components
{
    [SelectionBase]
    [RequireComponent(typeof(HealthComponent))]
    public class Buildable : MonoBehaviour
    {
        public BuildData Data;
        public int ID;

        public virtual void Start()
        {
            var health = GetComponent<HealthComponent>();
            health.MaxHealth = Data.Health;
            ID = GetInstanceID();
            health.OnDeath += DestroyEvent;
        }

        private void DestroyEvent(HealthComponent health)
        {
            Destroy();
        }

        public void Destroy()
        {
            var health = GetComponentInChildren<HealthBarComponent>();
            if (health != null) health.Hide();
            var dc = gameObject.AddComponent<DestroyComponent>();
            dc.size = Data.Size.y;
        }
        
    }
}
