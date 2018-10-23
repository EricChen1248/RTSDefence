using Scripts.Helpers;
using UnityEngine;

namespace Scripts.GUI
{
    [DefaultExecutionOrder(999)]
    public class FloatingUIButton : MonoBehaviour {

        public void ClickClose()
        {
            Pool.ReturnToPool("FloatingUI", transform.parent.gameObject);
        }
    }
}
