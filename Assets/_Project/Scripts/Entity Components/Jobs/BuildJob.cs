using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly GhostModelScript _ghost;
        private readonly Queue<RecipeItem> _recipe;

        private BuildJobPhase _currentPhase;
        private GameObject _resourceHolder;

        public BuildJob(GhostModelScript ghost)
        {
            _ghost = ghost;
            _recipe = new Queue<RecipeItem>(ghost.Recipe);
            _currentPhase = BuildJobPhase.CollectingResources;
        }

        private enum BuildJobPhase
        {
            Building,
            CollectingResources,
            DeliveringResources
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
            Worker.Agent.SetDestination(_ghost.transform.position);
            var collider = _ghost.GetComponent<Collider>();
            while (true)
            {
                var colliders = Physics.OverlapSphere(Worker.transform.position, 2f,
                    1 << LayerMask.NameToLayer("GhostModel"));
                if (colliders.Contains(collider)) break;

                yield return new WaitForFixedUpdate();
            }

            Worker.Agent.SetDestination(Worker.transform.position);
            yield return new WaitForSeconds(1);

            var comp = _resourceHolder.GetComponent<ResourceHolderComponent>();
            _ghost.DepositResources(comp.HeldResource, comp.HeldCount);
            Object.Destroy(_resourceHolder);

            _currentPhase = _recipe.Count > 0 ? BuildJobPhase.CollectingResources : BuildJobPhase.Building;
            Worker.DoWork(this);
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
                    Worker = null;
                    yield break;
                }

                _recipe.Dequeue();
                ResourceController.AddResource(res.Resource, -res.Amount);

                Worker.Agent.SetDestination(CoreController.Instance.CoreGameObject.transform.position);
                while (true)
                {
                    var colliders = Physics.OverlapSphere(Worker.transform.position, 2f,
                        1 << LayerMask.NameToLayer("Core"));
                    if (colliders.Length > 0) break;
                    yield return new WaitForFixedUpdate();
                }

                Worker.Agent.SetDestination(Worker.transform.position);
                yield return new WaitForSeconds(1);

                _resourceHolder =
                    Object.Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Resource Holder"),
                        Worker.transform);

                _resourceHolder.GetComponent<ResourceHolderComponent>().ChangeResources(res.Resource, res.Amount);

                _resourceHolder.transform.localPosition = Vector3.up * 0.5f + Vector3.forward * 0.5f;
            }

            _currentPhase = BuildJobPhase.DeliveringResources;
            Worker.DoWork(this);
        }

        private IEnumerator Build()
        {
            Worker.Agent.SetDestination(Worker.transform.position);
            Worker.GetComponent<Animator>().SetBool("Building", true);
            if (_resourceHolder != null)
            {
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
            Worker.GetComponent<Animator>().SetBool("Building", false);
        }

        #endregion
    }
}