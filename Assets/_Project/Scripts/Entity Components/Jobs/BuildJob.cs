using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Buildable_Components;
using Scripts.Controllers;
using Scripts.Helpers;
using Scripts.Resources;
using Scripts.Scriptable_Objects;
using UnityEngine;
namespace Scripts.Entity_Components.Jobs
{
    public class BuildJob : Job
    {
        private readonly Queue<RecipeItem> _recipe;
        private readonly GhostModelScript _ghost;

        private BuildJobPhase _currentPhase;
        private GameObject _resourceHolder;
        
        public BuildJob(GhostModelScript ghost)
        {
            _ghost = ghost;
            _recipe = new Queue<RecipeItem>(ghost.Recipe);
            _currentPhase = BuildJobPhase.CollectingResources;
        }

        #region IJob Interface Methods

        public override IEnumerator DoJob()
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

        public override void CancelJob()
        {
            _ghost.Cancel();
            CompleteJob();
        }

        #endregion

        #region Sub Jobs

        private IEnumerator DeliveringResource()
        {
            Worker.Agent.destination = _ghost.transform.position;
            while (true)
            {
                var distanceToTarget = (Worker.transform.position - Worker.Agent.destination).sqrMagnitude;
                if (distanceToTarget < 2f) break;

                yield return new WaitForFixedUpdate();
            }

            Worker.Agent.destination = Worker.Agent.nextPosition;

            var comp = _resourceHolder.GetComponent<ResourceHolderComponent>();
            _ghost.DepositResources(comp.HeldResource, comp.HeldCount);
            Pool.ReturnToPool("Resource Holder", _resourceHolder);

            _currentPhase = _recipe.Count > 0 ? BuildJobPhase.CollectingResources : BuildJobPhase.Building;
            Worker.DoWork(this);
        }

        private IEnumerator CollectingResources()
        {
            if (_recipe.Count > 0)
            {
                Worker.Agent.destination = CoreController.Instance.CoreGameObject.transform.position;
                while (true)
                {
                    var distanceToTarget = (Worker.transform.position - Worker.Agent.destination).sqrMagnitude;
                    if (distanceToTarget < 2f) break;

                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(1);

                // TODO : Add Take Resource.
                _resourceHolder = Pool.Spawn("Resource Holder");
                if (_resourceHolder == null)
                {
                    _resourceHolder = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Resource Holder"));
                }

                var res = _recipe.Dequeue();
                _resourceHolder.GetComponent<ResourceHolderComponent>().ChangeResources(res.Resource, res.Amount);

                _resourceHolder.transform.parent = Worker.gameObject.transform;
                _resourceHolder.transform.position = Worker.transform.position + Vector3.up * 2;
            }

            _currentPhase = BuildJobPhase.DeliveringResources;
            Worker.DoWork(this);
        }

        private IEnumerator Build()
        {
            if (_resourceHolder != null)
            {
                Pool.ReturnToPool("Resource Holder", _resourceHolder);
                _resourceHolder = null;
            }

            while (_ghost.WorkLeft > 0)
            {
                yield return new WaitForSeconds(0.1f);
                _ghost.DoWork();
            }
            CoreController.BuildController.Build(_ghost.transform.gameObject);
            CompleteJob();
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
