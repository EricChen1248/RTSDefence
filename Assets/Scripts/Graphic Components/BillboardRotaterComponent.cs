using UnityEngine;

namespace Graphic_Components
{
    public class BillboardRotaterComponent : MonoBehaviour
    {
        private Camera _camera;

        // Use this for initialization
        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
}
