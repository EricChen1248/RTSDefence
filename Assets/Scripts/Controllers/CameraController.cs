﻿using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _lastMouse;
        private bool _middleButtonDown;
        private Camera _camera;
        
        private const float MovementChange = 10;
        public Vector3 MinimumHeight = new Vector3(0, 1, 0);
        public float MaximumHeight = 10f;

        public void Start()
        {
            _camera = Camera.main;
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
            if (transform.eulerAngles.x + scrollChange <= 80 && transform.eulerAngles.x + scrollChange >= 30)
            {
                transform.RotateAround(transform.position, transform.right, scrollChange);
                _camera.orthographicSize += scrollChange;
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
    }
}
