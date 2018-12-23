using UnityEngine;

namespace Scripts.Towers
{
    public abstract class AmmoBase : MonoBehaviour
    {
        public Transform Target;
        public LayerMask Layer;
        public Transform Parent;
        public int Damage;
        public abstract void Fire();
    }
}
