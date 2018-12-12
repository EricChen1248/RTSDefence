using System;
using System.Collections;
using System.Linq;
using Scripts.Entity_Components.Misc;
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
        
        #region Targetting Functions

        private void TargetFirst()
        {
            if (!_inRangeTargets.Contains(_currentTarget))
            {
                _currentTarget = null;
            }

            if (_currentTarget != null) return;

            StopCoroutine(_shootingRoutine);
            _currentTarget = _inRangeTargets.FirstOrDefault();
            if (_currentTarget == null) return;

            StartCoroutine(_shootingRoutine);
        }

        private void TargetDistance(bool nearest)
        {
            Collider curTarget = null;
            var curDistance = nearest ? float.PositiveInfinity : 0.0f;
            foreach (var target in _inRangeTargets)
            {
                var dist = Vector3.Distance(transform.position, target.transform.position);
                if (nearest)
                {
                   if (!(dist < curDistance)) continue;
                }
                else
                {
                    if (!(dist > curDistance)) continue;
                }

                curTarget = target;
                curDistance = dist;
            }

            if (curTarget == null) return;
            if (_currentTarget == curTarget) return;

            StopCoroutine(_shootingRoutine);
            _currentTarget = curTarget;
            StartCoroutine(_shootingRoutine);
        }

        private void TargetHealth(bool highest)
        {
            Collider curTarget = null;
            var targetHealth = highest ? 0.0f : float.PositiveInfinity;
            foreach (var target in _inRangeTargets)
            {
                var healthComp = target.GetComponent<HealthComponent>();
                var health = (float)healthComp.Health / healthComp.MaxHealth;
                if (highest)
                {
                    if (!(health > targetHealth)) continue;
                }
                else
                {
                    if (!(health < targetHealth)) continue;
                }

                curTarget = target;
                targetHealth = health;
            }

            if (curTarget == null) return;
            if (_currentTarget == curTarget) return;

            StopCoroutine(_shootingRoutine);
            _currentTarget = curTarget;
            StartCoroutine(_shootingRoutine);

        }

        #endregion
        #region Coroutines
        
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

        private IEnumerator CheckEnterRange()
        {
            while (true)
            {
                _inRangeTargets = Physics.OverlapSphere(transform.position, Data.Range, RaycastHelper.LayerMaskDictionary["Enemies"]);

                switch (Data.AiState)
                {
                    case AiStates.FIRST:
                        TargetFirst();
                        break;
                    case AiStates.NEAREST:
                        TargetDistance(nearest: true);
                        break;
                    case AiStates.FURTHEST:
                        TargetDistance(nearest: false);
                        break;
                    case AiStates.WEAKEST:
                        TargetHealth(highest: false);
                        break;
                    case AiStates.STRONGEST:
                        TargetHealth(highest: true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                for (var i = 0; i < 10; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        #endregion
    }
}
