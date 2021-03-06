﻿using UnityEngine;

namespace Scripts.Graphic_Components
{
    public class SpinComponent : MonoBehaviour
    {
        public Vector3 Rotation = new Vector3(0, 1, 0);
        public float RotationSpeed = 1.5f;

        public void FixedUpdate()
        {
            transform.RotateAround(transform.position, Rotation, RotationSpeed);
        }
    }
}