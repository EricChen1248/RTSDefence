using Scripts.Entity_Components;
using Scripts.Entity_Components.Misc;
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
            GetComponent<HealthComponent>().MaxHealth = Data.Health;
            ID = GetInstanceID();
        }
    }
}
