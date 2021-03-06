﻿using System.Collections;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Graphic_Components
{
    public class OrbitComponent : MonoBehaviour
    {
        private float _rotationSpeed;

        public Transform Center;

        public Vector3 Normal;
        public bool RandomNormal;
        public MinMaxPair SpeedRange;

        private void Start()
        {
            _rotationSpeed = SpeedRange.RandomValue * 1000 * (Random.Range(0, 2) == 0 ? 1f : -1f);
            if (RandomNormal) StartCoroutine(ChangeNormal());
        }

        private void FixedUpdate()
        {
            transform.RotateAround(Center.position, Normal, _rotationSpeed * Time.deltaTime);
        }

        private IEnumerator ChangeNormal()
        {
            while (true)
            {
                Normal += new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
                _rotationSpeed = _rotationSpeed / Mathf.Abs(_rotationSpeed) * SpeedRange.RandomValue * 1000;
                yield return new WaitForSeconds(Random.Range(0f, 2f));
            }
        }
    }
}