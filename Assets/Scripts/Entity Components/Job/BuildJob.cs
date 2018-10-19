using System;
using System.Collections;
using System.Collections.Generic;
using Buildable_Components;
using Controllers;
using Entity_Components.Friendlies;
using Helpers;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace Entity_Components.Job
{
    public class BuildJob : IJob
    {
        private readonly Queue<RecipeItem> recipe = new Queue<RecipeItem>();
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

        public IEnumerator DoJob()
        {
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

        private IEnumerator DeliveringResource()
        {
            _sender.Agent.destination = _ghost.transform.position;
            while (true)
            {
                if (_sender.Agent.pathStatus == NavMeshPathStatus.PathComplete &&
                    Math.Abs(_sender.Agent.remainingDistance) < 0.1f) break;
                yield return new WaitForFixedUpdate();
            }

            _sender.DoingJob = false;
            _currentPhase = recipe.Count > 0 ? BuildJobPhase.CollectingResources : BuildJobPhase.Building;
        }

        private IEnumerator CollectingResources()
        {
            if (recipe.Count > 0)
            {
                _sender.Agent.destination = CoreController.Instance.CoreGameObject.transform.position;
                while (true)
                {
                    if (_sender.Agent.pathStatus == NavMeshPathStatus.PathComplete &&
                        Math.Abs(_sender.Agent.remainingDistance) < 0.1f) break;
                    yield return new WaitForFixedUpdate();
                }

                yield return new WaitForSeconds(1);

                // TODO : Add Take Resource.
                _resourceHolder = Pool.Spawn("Resource Holder");
                if (_resourceHolder == null)
                {
                    _resourceHolder = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Entities/Resource Holder")) as GameObject;
                }
                
                if (_resourceHolder != null)
                {
                    _resourceHolder.GetComponent<ResourceHolderComponent>().SetRecipeItem(recipe.Dequeue());
                }
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
        
        public void CancelJob()
        {
            throw new System.NotImplementedException();
        }

        private enum BuildJobPhase
        {
            Building,
            CollectingResources,
            DeliveringResources
        }
    }

}
