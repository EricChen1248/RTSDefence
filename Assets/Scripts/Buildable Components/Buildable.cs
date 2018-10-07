using Entity_Components;
using Scriptable_Objects;
using UnityEngine;

namespace Buildable_Components
{
    [SelectionBase]
    [RequireComponent(typeof(HealthComponent))]
    public class Buildable : MonoBehaviour
    {
        public BuildData Data;
        public int ID;

        public virtual void Start()
        {
            GetComponent<HealthComponent>().Health = Data.Health;
            ID = GetInstanceID();
        }
    }
}
