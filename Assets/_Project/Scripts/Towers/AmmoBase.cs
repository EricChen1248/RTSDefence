using UnityEngine;

namespace Scripts.Towers
{
    public abstract class AmmoBase : MonoBehaviour
    {
        public int Damage;
        public LayerMask Layer;
        public Transform Parent;
        public Transform Target;
        public abstract void Fire();
    }
}