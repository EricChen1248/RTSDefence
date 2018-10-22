using System.Collections;
using UnityEngine;

namespace Controllers
{
    [DefaultExecutionOrder(0)]
    public class CameraController : MonoBehaviour
    {
        public GameObject MainCamHolder;
        public Camera MinimapCamera;

        public Camera MainCamera { get; private set; }

        private Vector3 _lastMouse;
        private bool _middleButtonDown;
        
        private const float MovementChange = 10;
        public Vector3 MinimumHeight = new Vector3(0, 1, 0);
        public float MaximumHeight = 10f;

        public void Start()
        {
            MainCamera = Camera.main;
            CoreController.CameraController = this;
        }

        public void FixedUpdate()
        {
            // Mouse middle click 
            if (_middleButtonDown) 
            {
                var change = Input.mousePosition - _lastMouse;
                transform.RotateAround(transform.position, Vector3.up, change.x);

                _middleButtonDown = Input.GetMouseButton(2);
                _lastMouse = Input.mousePosition;
            }
            else if (Input.GetMouseButton(2))
            {
                _middleButtonDown = true;
                _lastMouse = Input.mousePosition;
            }


            // Mouse scroll
            var scrollChange = Input.mouseScrollDelta.y * -2f;
            // Clamp to top and bottom angle
            if (MainCamHolder.transform.eulerAngles.x + scrollChange <= 80 && MainCamHolder.transform.eulerAngles.x + scrollChange >= 30)
            {
                MainCamHolder.transform.RotateAround(transform.position, transform.right, scrollChange);
                MainCamera.orthographicSize += scrollChange;
            }

            // Keyboard commands
            transform.position += GetBaseInput() * Time.deltaTime * MovementChange;
        }

        private Vector3 GetBaseInput()
        {
            var pVelocity = new Vector3();
            if (Input.GetKey(KeyCode.D))
            {
                pVelocity += transform.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pVelocity -= transform.right;
            }

            if (Input.GetKey(KeyCode.W))
            {
                pVelocity += Vector3.Normalize(transform.forward - new Vector3(0, transform.forward.y, 0)) * 2;
            }

            if (Input.GetKey(KeyCode.S))
            {
                pVelocity -= Vector3.Normalize(transform.forward - new Vector3(0, transform.forward.y, 0)) * 2;
            }

            return pVelocity;
        }

        private IEnumerator _moveAnimator;
        public void MoveCam(float xChange, float yChange)
        {
            if (_moveAnimator != null)
            {
                StopCoroutine(_moveAnimator);
            }

            _moveAnimator = MoveEnumerator(xChange, yChange);
            StartCoroutine(_moveAnimator);
        }

        private IEnumerator MoveEnumerator(float xChange, float yChange)
        {
            //xChange /= 10;
            //yChange /= 10;

            var startPos = transform.position;
            var endPos = transform.position + transform.right * xChange +
                         Vector3.Normalize(transform.forward - new Vector3(0, transform.forward.y, 0)) * 2 * yChange;

            for (float i = 0; i < 10; i++)
            {
                transform.position = Vector3.Lerp(startPos, endPos, i / 10);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
