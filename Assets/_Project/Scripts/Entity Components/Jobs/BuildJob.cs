using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Buildable_Components;
using Scripts.Controllers;
using Scripts.Resources;
using Scripts.Scriptable_Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Entity_Components.Jobs
{
    public class BuildJob : Job
    {
        private readonly Queue<RecipeItem> _recipe;
        private readonly GhostModelScript _ghost;

        private BuildJobPhase _currentPhase;
        private GameObject _resourceHolder;

        public bool AtDestination;
        
        public BuildJob(GhostModelScript ghost)
        {
            _ghost = ghost;
            _recipe = new Queue<RecipeItem>(ghost.Recipe);
            _currentPhase = BuildJobPhase.CollectingResources;
        }

        #region IJob Interface Methods

        public override IEnumerator DoJob()
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
            AtDestination = false;

            while (!AtDestination)
            {
                yield return new WaitForFixedUpdate();
            }

            Worker.Agent.isStopped = true;
            yield return new WaitForSeconds(1);

            var comp = _resourceHolder.GetComponent<ResourceHolderComponent>();
            _ghost.DepositResources(comp.HeldResource, comp.HeldCount);
            Object.Destroy(_resourceHolder);

            _currentPhase = _recipe.Count > 0 ? BuildJobPhase.CollectingResources : BuildJobPhase.Building;
            Worker.DoWork(this);
            Worker.Agent.isStopped = false;
        }

        private IEnumerator CollectingResources()
        {
            if (_recipe.Count > 0)
            {
                var res = _recipe.Peek();

                // If not enough resources.
                if (ResourceController.ResourceCount[res.Resource] < res.Amount)
                {
                    // Wait one update so the update isn't full of failed jobs.
                    yield return new WaitForFixedUpdate();

                    JobController.AddJob(this);
                    Worker.CompleteJob();
                    yield break;
                }

                ResourceController.AddResource(res.Resource, -res.Amount);

                Worker.Agent.destination = CoreController.Instance.CoreGameObject.transform.position;
                while (true)
                {
                    var distanceToTarget = (Worker.transform.position - CoreController.Instance.CoreGameObject.transform.position).sqrMagnitude;
                    if (distanceToTarget < 4f) break;

                    yield return new WaitForFixedUpdate();
                }

                yield return new WaitForSeconds(1);


                _resourceHolder = Object.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Resource Holder"));

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
                Object.Destroy(_resourceHolder);
                Object.Destroy(_resourceHolder);
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
