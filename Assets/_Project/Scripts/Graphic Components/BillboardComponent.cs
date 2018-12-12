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
            transform.eulerAngles = _camera.transform.eulerAngles;
            //transform.eulerAngles = new Vector3(_camera.transform.eulerAngles.x, _camera.transform.parent.gameObject.transform.eulerAngles.y, transform.eulerAngles.z);
            //transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
        }
    }
}