using System.Collections.Generic;
using UnityEngine;

namespace Entity_Components.Ais
{
    public class Patroller : MonoBehaviour
    {
        public List<PatrolPoint> PatrolPoints;
        private int _currentPoint;

        // Use this for initialization
        public void Start ()
        {
            PatrolPoints = new List<PatrolPoint>();
        }

        public Vector3 GetNextPatrol()
        {
            _currentPoint = (++_currentPoint) % PatrolPoints.Count;
            return PatrolPoints[_currentPoint].GetPoint;
        }

        public void AddPoint(Transform t)
        {
            PatrolPoints.Add(new PatrolPoint(t));
        }

        public void AddPoint(Vector3 v)
        {
            PatrolPoints.Add(new PatrolPoint(v));
        }

        public struct PatrolPoint
        {
            private readonly Transform _transform;
            private readonly Vector3 _vector3;
            private readonly bool _usesTransform;

            public Vector3 GetPoint => _usesTransform ? _transform.position : _vector3;

            public PatrolPoint(Transform t)
            {
                _transform = t;
                _vector3 = Vector3.zero;
                _usesTransform = true;
            }

            public PatrolPoint(Vector3 v)
            {
                _transform = null;
                _vector3 = v;
                _usesTransform = false;
            }
        }

    }
}
