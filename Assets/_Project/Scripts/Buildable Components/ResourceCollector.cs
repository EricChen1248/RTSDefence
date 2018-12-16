using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.GUI;
using Scripts.Interface;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Buildable_Components
{
    public class ResourceCollector : Buildable, IClickable
    {
        public ResourceTypes Type;
        public GameObject Gatherer;

        public int CollectionRadius;
        public List<ResourceNode> Nodes;
        private readonly List<ResourceGatherer> Gatherers = new List<ResourceGatherer>();
        private GameObject _range;


        private ResourceNode CurrentNode;

        public override void Start()
        {
            base.Start();
            GetResourceInRange();
            GetClosestNode();

            StartCoroutine(SpawnGatherer(3));
        }

        public void OnDestroy()
        {
            if (CurrentNode != null)
            {
                CurrentNode.Collectors.Remove(this);
            }
        }

        private void GetResourceInRange()
        {
            var overlaps = Physics.OverlapSphere(transform.position, CollectionRadius, 1 << LayerMask.NameToLayer("Resource"));

            Nodes = new List<ResourceNode>();
            foreach (var overlap in overlaps)
            {
                var node = overlap.GetComponent<ResourceNode>();
                if (node != null)
                {
                    if (node.Type == Type)
                    {
                        Nodes.Add(node);
                        node.Collectors.Add(this);
                    }
                }
            }
        }

        public IEnumerator SpawnGatherer(int count)
        {
            var prefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Friendlies/Gatherer");
            prefab.transform.position = transform.forward * -2.5f;

            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(prefab, transform);
                obj.GetComponent<NavMeshAgent>().enabled = true;

                yield return new WaitForSeconds(1);

                var gatherer = obj.GetComponent<ResourceGatherer>();
                Gatherers.Add(gatherer);
                gatherer.Collector = this;
                gatherer.Node = CurrentNode;
                gatherer.GatherNewResource();
                yield return new WaitForSeconds(1);
            }
        }

        private void GetClosestNode()
        {
            var minDist = float.MaxValue;
            foreach (var node in Nodes)
            {
                var dist = (node.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist)
                {
                    CurrentNode = node;
                    minDist = dist;
                }
            }
        }
        public void NotifyNodeDestroy(ResourceNode node)
        {
            Nodes.Remove(node);
            if (CurrentNode != node) return;
            GetClosestNode();
            foreach (var gatherer in Gatherers)
            {
                gatherer.Node = CurrentNode;
            }
        }

        public void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            HasFocus = true;
            _range = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Map Objects/RangeShower"), transform);
            _range.transform.localScale = new Vector3(CollectionRadius * 2, 5, CollectionRadius * 2);

            var omg = ObjectMenuGroupComponent.Instance;

            omg.ResetButtons();
            omg.SetButton(1, "Destroy", Destroy);
            omg.Show();
        }

        public void LostFocus()
        {
            HasFocus = false;
            Destroy(_range);
        }

        public void Destroy()
        {
            foreach (var gatherer in Gatherers)
            {
                Destroy(gatherer.gameObject);
            }
            Destroy(true);
        }
        
        public void RightClick()
        {

        }
    }
}
