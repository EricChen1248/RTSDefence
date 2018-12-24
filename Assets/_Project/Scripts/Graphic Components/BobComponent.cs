using System;
using UnityEngine;

namespace Scripts.Graphic_Components
{
    public class BobComponent : MonoBehaviour
    {
        private int _direction = 1;

        private float _initialY;
        public float BobMax = 0.5f;
        public float BobSpeed = 1.5f;

        public void Start()
        {
            _initialY = transform.localPosition.y;
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            transform.localPosition += new Vector3(0, _direction * BobSpeed / 100f, 0);
            _direction *= Math.Abs(transform.localPosition.y - _initialY) > BobMax ? -1 : 1;
        }
    }
}