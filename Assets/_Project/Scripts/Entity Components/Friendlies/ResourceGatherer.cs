using Scripts.Buildable_Components;
using Scripts.Controllers;
using Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ResourceGatherer : MonoBehaviour
{

    public ResourceCollector Collector;
    public ResourceNode Node;

    private IEnumerator _collectionRoutine;
    private GameObject Holders;
    private int resourceMask;
    private int collectorMask;

    private Resource resource = new Resource();

    private bool _delivering = false;
    // Use this for initialization
    public void Start()
    {
        _collectionRoutine = Collect();

        resourceMask = 1 << LayerMask.NameToLayer("Resource");
        collectorMask = 1 << LayerMask.NameToLayer("Resource Collection");
    }

    private IEnumerator Collect()
    {
        var agent = GetComponent<NavMeshAgent>();
        var holder = Resources.Load<GameObject>("Prefabs/Entities/Resource Holder");

        while (true)
        {
            if (!_delivering)
            {
                agent.isStopped = false;
                agent.destination = Node.transform.position;
                var oldNode = Node;
                while (true)
                { 
                    if (AtResourceNode()) break;
                    if (oldNode != Node) agent.destination = Node.transform.position;
                    yield return new WaitForFixedUpdate();
                }
                resource = Node.GetResource();
                Holders = Instantiate(holder, transform);
                Holders.transform.localPosition = Vector3.up * 1.5f;
                Holders.GetComponent<ResourceHolderComponent>().ChangeResources(resource.Type, resource.Count);
                agent.isStopped = true;
                _delivering = true;
                yield return new WaitForSeconds(1);
            }
            agent.isStopped = false;
            agent.destination = Collector.transform.position;

            while (true)
            {
                if (AtCollector()) break;
                yield return new WaitForFixedUpdate();
            }

            agent.isStopped = true;
            _delivering = false;
            ResourceController.AddResource(resource.Type, resource.Count);

            yield return new WaitForSeconds(1);
            Destroy(Holders);
            Holders = null;

            yield return new WaitForSeconds(1);

        }
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
            foreach (var col in overlaps)
            {
                if (col.GetComponent<ResourceNode>() == Node)
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool AtCollector()
    {
        var overlaps = Physics.OverlapSphere(transform.position, 1f, collectorMask);
        foreach (var col in overlaps)
        {
            if (col.GetComponent<ResourceCollector>() == Collector)
            {
                return true;
            }
        }
        return false;
    }
}
