using UnityEngine;

namespace Buildable_Components
{
    public class GhostModelScript : MonoBehaviour
    {
        public bool CanBuild()
        {
            return Physics.OverlapBox(transform.position, transform.localScale * 0.49f, transform.rotation).Length == 0;
        }
    }
}
