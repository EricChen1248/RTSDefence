using System;
using System.Collections;
using System.Linq;
using Scripts.Buildable_Components;
using Scripts.Controllers;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Friendlies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ResourceGatherer : MonoBehaviour
    {
        public ResourceCollector Collector;
        public bool SpawnedFromCollector;

        private Animator _animator;
        private IEnumerator _collectionRoutine;

        private bool _delivering;

        private int _collectorMask;
        private GameObject _holders;
        public ResourceNode Node;

        private Resource _resource;

        private int _resourceMask;

        // Use this for initialization
        public void Start()
        {
            if (!SpawnedFromCollector)
            {
                Collector = GetComponentInParent<ResourceCollector>();
                Collector.Add(this);
            }

            GetComponent<NavMeshAgent>().enabled = true;

            _collectionRoutine = Collect();
            _animator = GetComponent<Animator>();

            _resourceMask = 1 << LayerMask.NameToLayer("Resource");
            _collectorMask = 1 << LayerMask.NameToLayer("Resource Collection");
        }

        private IEnumerator Collect()
        {
            var agent = GetComponent<NavMeshAgent>();
            var holder = UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Resource Holder");

            while (Node != null)
            {
                if (!_delivering)
                {
                    agent.isStopped = false;
                    _animator.SetBool("Walking", true);
                    agent.SetDestination(Node.transform.position);
                    var oldNode = Node;
                    while (Node != null)
                    {
                        if (AtResourceNode()) break;
                        if (oldNode != Node) agent.SetDestination(Node.transform.position);
                        yield return new WaitForFixedUpdate();
                    }

                    if (Node != null)
                    {
                        _resource = Node.GetResource();
                        _holders = Instantiate(holder, transform);
                        _holders.transform.localPosition = Vector3.up * 0.5f + Vector3.forward * 0.5f;
                        _holders.GetComponent<ResourceHolderComponent>().ChangeResources(_resource.Type, _resource.Count);
                        agent.isStopped = true;
                        _animator.SetBool("Walking", false);
                        _delivering = true;
                    }
                }

                if (Node == null) continue;
                yield return new WaitForSeconds(1);
                agent.isStopped = false;
                _animator.SetBool("Walking", true);
                agent.SetDestination(Collector.transform.position);

                while (true)
                {
                    if (AtCollector()) break;
                    yield return new WaitForFixedUpdate();
                }

                agent.isStopped = true;
                _animator.SetBool("Walking", false);
                _delivering = false;
                ResourceController.AddResource(_resource.Type, _resource.Count);

                yield return new WaitForSeconds(1);
                Destroy(_holders);
                _holders = null;

                yield return new WaitForSeconds(1);
            }

            agent.isStopped = false;
            _animator.SetBool("Walking", true);
            agent.SetDestination(Collector.transform.position);

            while (true)
            {
                if (AtCollector()) break;
                yield return new WaitForFixedUpdate();
            }

            _animator.SetBool("Walking", false);
            agent.isStopped = true;
        }

        public void GatherNewResource()
        {
            StartCoroutine(_collectionRoutine);
        }

        private bool AtResourceNode()
        {
            try
            {
                var overlaps = Physics.OverlapSphere(transform.position, 1f, _resourceMask);
                return overlaps.Any(col => col.GetComponent<ResourceNode>() == Node);
            }
            catch (Exception)
            {
                // Resource Node is gone, return true
                return true;
            }
        }

        private bool AtCollector()
        {
            var overlaps = Physics.OverlapSphere(transform.position, 1f, _collectorMask);
            return overlaps.Any(col => col.GetComponent<ResourceCollector>() == Collector);
        }
    }
}