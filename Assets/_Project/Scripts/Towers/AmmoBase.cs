using UnityEngine;

namespace Scripts.Towers
{
    public abstract class AmmoBase : MonoBehaviour
    {
        public Transform Target;

        public abstract void Fire();
    }
}
