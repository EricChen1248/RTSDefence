using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Buildable_Components;
using Scripts.Entity_Components.Misc;
using Scripts.GUI;
using Scripts.Navigation;
using Scripts.Scriptable_Objects;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Scripts.Towers
{
    [DefaultExecutionOrder(0)]
    public class TurretBase : Buildable
    {
        public GameObject FireSpot;
        public TurretData TurretData;

        private Collider[] _inRangeTargets;
        private Collider _currentTarget;

        private Vector3 _lastTargetPosition = Vector3.zero;
        private Quaternion _targetRotation;

        private readonly IEnumerator _shootingRoutine;

        private GameObject _range;

        private AiStates State;

        public TurretBase()
        {
            _shootingRoutine = Shoot();
        }

        public override void Start()
        {
            base.Start();
            StartCoroutine(CheckEnterRange());
            State = TurretData.AiState;
        }

        public override void Focus()
        {
            base.Focus();
            _range = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Map Objects/RangeShower"), transform);
            _range.transform.localScale = new Vector3(TurretData.Range * 2, 5, TurretData.Range * 2);

            UpdateGui();
        }

        private readonly Dictionary<AiStates, string> StateString = new Dictionary<AiStates, string>()
        {
            {AiStates.FIRST,"First"},        
            {AiStates.NEAREST,"Nearest"},
            {AiStates.FURTHEST,"Furthest"},
            {AiStates.WEAKEST,"Weakest"},
            {AiStates.STRONGEST,"Strongest"}
        };

        private void UpdateGui()
        {
            var omg = ObjectMenuGroupComponent.Instance;
            omg.SetButton(1, StateString[State], ToggleFiringState);
            omg.SetButtonImage(1, UnityEngine.Resources.Load<Texture>("Sprites/reticle"));
        }
        public override void LostFocus()
        {
            base.LostFocus();
            Object.Destroy(_range);
        }

        public void ToggleFiringState()
        {
            switch (State)
            {
                case AiStates.FIRST:
                    State = AiStates.NEAREST;
                    break;
                case AiStates.NEAREST:
                    State = AiStates.FURTHEST;
                    break;
                case AiStates.FURTHEST:
                    State = AiStates.WEAKEST;
                    break;
                case AiStates.WEAKEST:
                    State = AiStates.STRONGEST;
                    break;
                case AiStates.STRONGEST:
                    State = AiStates.FIRST;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateGui();
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
            while (_currentTarget != null)
            {
                var angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_currentTarget.transform.position - transform.position));
                if (angle < TurretData.FieldOfView)
                {
                    var go = TurretData.SpawnAmmo(FireSpot.transform);
                    var ammo = go.GetComponent<AmmoBase>();
                    go.transform.position = FireSpot.transform.position;
                    ammo.Layer = RaycastHelper.LayerMaskDictionary["Enemies"];
                    ammo.Target = _currentTarget.transform;
                    ammo.Fire();
                }


                yield return new WaitForSeconds(TurretData.FireRate);
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
                _inRangeTargets = Physics.OverlapSphere(transform.position, TurretData.Range, RaycastHelper.LayerMaskDictionary["Enemies"]);

                switch (State)
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
