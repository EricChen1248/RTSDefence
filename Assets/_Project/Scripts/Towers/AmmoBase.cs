using UnityEngine;

namespace Scripts.Towers
{
    public abstract class AmmoBase : MonoBehaviour
    {
        public Transform Target;
        public LayerMask Layer;

        public abstract void Fire();
    }
}
