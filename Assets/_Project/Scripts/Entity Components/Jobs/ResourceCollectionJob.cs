using System;
using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Friendlies;
using Scripts.Resources;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Entity_Components.Jobs
{
    public class ResourceCollectionJob : Job
    {
        // private resource collection;
        private readonly PlayerComponent _sender;
        private readonly NodeManager _node;

        private JobPhase _currentPhase;

        public ResourceTypes HeldResourceType;
        public int HeldCount;

        private bool _hasResources;

        private GameObject _rhp;

        public ResourceCollectionJob(PlayerComponent sender, NodeManager node)
        {
            _sender = sender;
            _node = node;
            _currentPhase = JobPhase.MovingToResource;
        }

        public override IEnumerator DoJob()
        {
            Debug.Log(_currentPhase);
            switch (_currentPhase)
            {
                case JobPhase.Collecting:
                    return CollectResources();
                case JobPhase.MovingToBase:
                    return MoveToBase();
                case JobPhase.Depositing:
                    return DepositResource();
                case JobPhase.MovingToResource:
                    return MoveToResource();
                default:
                    throw new ArgumentOutOfRangeException($"{_sender} has invalid job phase {_currentPhase}");
            }
        }
        

        public override void CancelJob()
        {
            throw new NotImplementedException();
        }

        private IEnumerator MoveToResource()
        {
            _sender.MoveToLocationOnGrid(_node.transform.position);
            
            var sqrRadius = _node.CollectionRadius * _node.CollectionRadius;
            while ((_sender.transform.position - _node.transform.position).sqrMagnitude > sqrRadius)
            {
                yield return new WaitForFixedUpdate();
            }
            _sender.DoingJob = false;
            _currentPhase = JobPhase.Collecting;
            _sender.Stop();
        }

        private IEnumerator CollectResources()
        {
            _hasResources = true;
            HeldResourceType = _node.ResourceType;

            _node.Collectors.Add(this);
            _rhp = Object.Instantiate(ResourceController.Instance.ResouceHolderPrefab, _sender.transform);
            _rhp.transform.localPosition = new Vector3(0, 2, 0);
            var component = _rhp.GetComponent<ResourceHolderComponent>();
            component.ChangeResources(HeldResourceType, 0);
            while (_hasResources)
            {
                var time = _node.GatherResource();
                yield return new WaitForSeconds(time);
                HeldCount++;
                component.HeldCount++;
            }

            _node.Collectors.Remove(this);
            _sender.DoingJob = false;
            _currentPhase = JobPhase.MovingToBase;
        }

        public void ResourcesGone()
        {
            _hasResources = false;
        }

        private IEnumerator DepositResource()
        {
            _rhp.GetComponent<ResourceHolderComponent>().MoveTo(_rhp.transform.position, CoreController.Instance.CoreGameObject.transform.position, 1.5f);
            yield return new WaitForSeconds(2);

            ResourceController.AddResource(HeldResourceType, HeldCount);
            Object.Destroy(_rhp);
            _sender.DoingJob = false;
            _sender.CurrentJob = null;
        }

        private IEnumerator MoveToBase()
        {
            var corePos = CoreController.Instance.CoreGameObject.transform.position;
            _sender.MoveToLocationOnGrid(corePos);
            while ((_sender.transform.position - corePos).sqrMagnitude > 11)
            {
                yield return new WaitForFixedUpdate();
            }

            _sender.DoingJob = false;
            _currentPhase = JobPhase.Depositing;
            _sender.Stop();
        }

        private enum JobPhase
        {
            Collecting,
            MovingToBase,
            Depositing,
            MovingToResource
        }
    }
}
