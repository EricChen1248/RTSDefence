using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Buildable_Components;
using Scripts.Controllers;
using Scripts.Entity_Components.Friendlies;
using Scripts.Helpers;
using Scripts.Resources;
using Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Job
{
    public class BuildJob : IJob
    {
        private readonly Queue<RecipeItem> _recipe = new Queue<RecipeItem>();
        private readonly PlayerComponent _sender;
        private readonly GhostModelScript _ghost;
        private readonly int _buildTime;

        private BuildJobPhase _currentPhase;
        private GameObject _resourceHolder;
        
        public BuildJob(PlayerComponent sender, int buildTime, GhostModelScript ghost)
        {
            _sender = sender;

            _buildTime = buildTime;
            _ghost = ghost;

            _currentPhase = BuildJobPhase.CollectingResources;
        }

        #region IJob Interface Methods

        public IEnumerator DoJob()
        {
            Debug.Log(_currentPhase);
            switch (_currentPhase)
            {
                case BuildJobPhase.Building:
                    return Build();
                case BuildJobPhase.CollectingResources:
                    return CollectingResources();
                case BuildJobPhase.DeliveringResources:
                    return DeliveringResource();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CancelJob()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Sub Jobs

        private IEnumerator DeliveringResource()
        {
            _sender.Agent.destination = _ghost.transform.position;
            while ((_sender.transform.position - _ghost.transform.position).sqrMagnitude > 2.5f)
            {
                yield return new WaitForFixedUpdate();
            }
            _sender.Stop();
            _sender.DoingJob = false;
            _currentPhase = _recipe.Count > 0 ? BuildJobPhase.CollectingResources : BuildJobPhase.Building;
        }

        private IEnumerator CollectingResources()
        {
            if (_recipe.Count > 0)
            {
                _sender.Agent.destination = CoreController.Instance.CoreGameObject.transform.position;
                while (true)
                {
                    if (_sender.Agent.pathStatus == NavMeshPathStatus.PathComplete &&
                        Math.Abs(_sender.Agent.remainingDistance) < 0.2f) break;
                    yield return new WaitForFixedUpdate();
                }

                yield return new WaitForSeconds(1);

                // TODO : Add Take Resource.
                _resourceHolder = Pool.Spawn("Resource Holder");
                if (_resourceHolder == null)
                {
                    _resourceHolder = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Resource Holder"));
                }
                
                _resourceHolder.GetComponent<ResourceHolderComponent>().ChangeResources(_recipe.Dequeue().Resource);
            }

            _sender.DoingJob = false;
            _currentPhase = BuildJobPhase.DeliveringResources;
        }

        private IEnumerator Build()
        {
            if (_resourceHolder != null)
            {
                Pool.ReturnToPool("Resource Holder", _resourceHolder);
                _resourceHolder = null;
            }

            for (var i = 0; i < _buildTime; i++)
            {
                yield return new WaitForSeconds(0.1f);
                _ghost.DoWork();
            }
            CoreController.BuildController.Build(_ghost.transform.gameObject);
            _sender.DoingJob = false;
            _sender.CurrentJob = null;
        }
        
        #endregion

        private enum BuildJobPhase
        {
            Building,
            CollectingResources,
            DeliveringResources
        }
    }

}
