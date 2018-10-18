using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Job
{
    internal class ResourceCollectionJob : IJob
    {
        // private resource collection;
        private Transform _resource;

        private JobPhase _currentPhase;

        public IEnumerator DoJob(PlayerComponent sender)
        {

            switch (_currentPhase)
            {
                case JobPhase.Collecting:
                    return CollectingResource(sender);
                    break;
                case JobPhase.MovingToBase:
                    break;
                case JobPhase.Depositing:
                    break;
                case JobPhase.MovingToResource:
                    return MoveToResource(sender);
                default:
                    throw new ArgumentOutOfRangeException($"{sender} has invalid job phase {_currentPhase}");
            }

            return null;
        }

        public IEnumerator ResumeJob(PlayerComponent sender)
        {
            throw new NotImplementedException();
        }

        private IEnumerator MoveToResource(PlayerComponent sender)
        {
            sender.MoveToLocation(_resource.position);
            while (true)
            {
                switch (sender.Agent.pathStatus)
                {
                    case NavMeshPathStatus.PathPartial:
                        break;
                    case NavMeshPathStatus.PathInvalid:
                    case NavMeshPathStatus.PathComplete:
                        sender.DoingJob = false;
                        _currentPhase = JobPhase.Collecting;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator CollectingResource(PlayerComponent sender)
        {
            // TODO : Update for resource collection time.
            yield return new WaitForSeconds(2);
            // TODO : Assign resources to person
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
