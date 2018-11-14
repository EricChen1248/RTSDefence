using UnityEngine;

namespace Scripts.Graphic_Components
{
    public class BillboardComponent : MonoBehaviour
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
