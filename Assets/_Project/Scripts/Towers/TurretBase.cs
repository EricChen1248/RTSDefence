using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Navigation;
using Scripts.Scriptable_Objects;
using UnityEngine;

namespace Scripts.Towers
{
    [DefaultExecutionOrder(0)]
    public class TurretBase : MonoBehaviour
    {
        public GameObject FireSpot;
        public TurretData Data;

        private Collider[] _inRangeTargets;
        private Collider _currentTarget;

        private Vector3 _lastTargetPosition = Vector3.zero;
        private Quaternion _targetRotation;

        private readonly IEnumerator _shootingRoutine;

        public TurretBase()
        {
            _shootingRoutine = Shoot();
        }

        public void Start()
        {
            StartCoroutine(CheckEnterRange());
        }

        private IEnumerator CheckEnterRange()
        {
            while (true)
            {
                _inRangeTargets = Physics.OverlapSphere(transform.position, 5, RaycastHelper.LayerMaskDictionary["Enemies"]);

                if (!_inRangeTargets.Contains(_currentTarget))
                {
                    _currentTarget = null;
                    
                }

                AssignTarget();

                for (var i = 0; i < 10; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private void AssignTarget()
        {
            if (_currentTarget != null) return;

            StopCoroutine(_shootingRoutine);
            _currentTarget = _inRangeTargets.FirstOrDefault();
            if (_currentTarget == null) return;

            StartCoroutine(_shootingRoutine);
        }

        private IEnumerator Shoot()
        {
            StartCoroutine(RotateToTarget());
            while (true)
            {
                var angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_currentTarget.transform.position - transform.position));
                if (angle < Data.FieldOfView)
                {
                    var go = Data.SpawnAmmo(FireSpot.transform);
                    var ammo = go.GetComponent<AmmoBase>();
                    go.transform.position = FireSpot.transform.position;
                    ammo.Target = _currentTarget.transform;
                    ammo.Fire();
                }


                yield return new WaitForSeconds(Data.FireRate);
            }
        }

        private IEnumerator RotateToTarget()
        {
            while (_currentTarget != null)
            {
                if (_lastTargetPosition != _currentTarget.transform.position)
                {
                    _lastTargetPosition = _currentTarget.transform.position;
                    var dir = _lastTargetPosition - transform.position;
                    _targetRotation = Quaternion.LookRotation(dir);
                    _targetRotation.x = 0;
                    _targetRotation.z = 0;
                }

                if (transform.rotation != _targetRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 100 * Time.deltaTime);
                }
                yield return new WaitForFixedUpdate();
            }
        }


    }
}
