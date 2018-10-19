using System;
using System.Collections;
using Controllers;
using Entity_Components.Friendlies;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Job
{
    internal class ResourceCollectionJob : IJob
    {
        // private resource collection;
        private PlayerComponent sender;
        private Transform _resource;

        private JobPhase _currentPhase;

        public ResourceCollectionJob(PlayerComponent sender)
        {
            this.sender = sender;
        }

        public IEnumerator DoJob()
        {

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
                    throw new ArgumentOutOfRangeException($"{sender} has invalid job phase {_currentPhase}");
            }

        }
        

        public void CancelJob()
        {
            throw new NotImplementedException();
        }

        private IEnumerator MoveToResource()
        {
            sender.MoveToLocation(_resource.position);
            bool moving = true;
            while (moving)
            {
                switch (sender.Agent.pathStatus)
                {
                    case NavMeshPathStatus.PathPartial:
                        break;
                    case NavMeshPathStatus.PathInvalid:
                    case NavMeshPathStatus.PathComplete:
                        sender.DoingJob = false;
                        _currentPhase = JobPhase.Collecting;
                        moving = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator CollectResources()
        {
            // TODO : Update for resource collection time.
            yield return new WaitForSeconds(2);
            // TODO : Assign resources to person
            sender.DoingJob = false;
            _currentPhase = JobPhase.MovingToBase;
        }

        private IEnumerator DepositResource()
        {
            // TODO : Deposit Resource
            yield return new WaitForSeconds(2);
            sender.DoingJob = false;
            _currentPhase = JobPhase.MovingToResource;
        }

        private IEnumerator MoveToBase()
        {
            sender.MoveToLocation(CoreController.Instance.CoreGameObject.transform.position);
            var moving = true;
            while (moving)
            {
                switch (sender.Agent.pathStatus)
                {
                    case NavMeshPathStatus.PathPartial:
                        break;
                    case NavMeshPathStatus.PathInvalid:
                    case NavMeshPathStatus.PathComplete:
                        sender.DoingJob = false;
                        _currentPhase = JobPhase.Depositing;
                        moving = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return new WaitForFixedUpdate();
            }
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
