using System;
using UnityEngine;

namespace Graphic_Components
{
    public class BobComponent : MonoBehaviour
    {
        public float BobMax = 0.5f;
        public float BobSpeed = 1.5f;

        private float _initialY;
        private int _direction = 1;
        public void Start()
        {
            _initialY = transform.position.y;
        }
        // Update is called once per frame
        public void FixedUpdate () {
            transform.position += new Vector3(0,_direction * BobSpeed / 100f, 0);
            _direction *= Math.Abs(transform.position.y - _initialY) > BobMax ? -1 : 1;

        }
    }
}
