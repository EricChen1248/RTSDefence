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
        private Animator _animator;
        private IEnumerator _collectionRoutine;

        private bool _delivering;

        public ResourceCollector Collector;
        private int collectorMask;
        private GameObject Holders;
        public ResourceNode Node;

        private Resource resource;

        private int resourceMask;

        // Use this for initialization
        public void Start()
        {
            _collectionRoutine = Collect();
            _animator = GetComponent<Animator>();

            resourceMask = 1 << LayerMask.NameToLayer("Resource");
            collectorMask = 1 << LayerMask.NameToLayer("Resource Collection");
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
                        resource = Node.GetResource();
                        Holders = Instantiate(holder, transform);
                        Holders.transform.localPosition = Vector3.up * 0.5f + Vector3.forward * 0.5f;
                        Holders.GetComponent<ResourceHolderComponent>().ChangeResources(resource.Type, resource.Count);
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
                ResourceController.AddResource(resource.Type, resource.Count);

                yield return new WaitForSeconds(1);
                Destroy(Holders);
                Holders = null;

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
                var overlaps = Physics.OverlapSphere(transform.position, 1.5f, resourceMask);
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
            var overlaps = Physics.OverlapSphere(transform.position, 1f, collectorMask);
            return overlaps.Any(col => col.GetComponent<ResourceCollector>() == Collector);
        }
    }
}