using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.Entity_Components.Friendlies;
using Scripts.GUI;
using Scripts.Interface;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Buildable_Components
{
    public class ResourceCollector : Buildable, IClickable
    {
        private readonly List<ResourceGatherer> Gatherers = new List<ResourceGatherer>();
        private GameObject _range;

        public int CollectionRadius;


        private ResourceNode CurrentNode;
        public GameObject Gatherer;
        public List<ResourceNode> Nodes;
        public ResourceTypes ResourceType;
        public Texture tex;

        public override void Focus()
        {
            base.Focus();

            _range = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Map Objects/RangeShower"), transform);
            _range.transform.localScale = new Vector3(CollectionRadius * 2, 5, CollectionRadius * 2);


            var omg = ObjectMenuGroupComponent.Instance;
            omg.SetButton(1, "Respawn", SpawnNew);
            omg.SetButtonImage(1, tex);
        }

        public override void LostFocus()
        {
            HasFocus = false;
            Object.Destroy(_range);
        }

        public override void Start()
        {
            base.Start();
            GetResourceInRange();
            GetClosestNode();

            StartCoroutine(SpawnGatherer(3));
        }

        public void OnDestroy()
        {
            if (CurrentNode != null) CurrentNode.Collectors.Remove(this);
        }

        private void GetResourceInRange()
        {
            var overlaps = Physics.OverlapSphere(transform.position, CollectionRadius,
                1 << LayerMask.NameToLayer("Resource"));

            Nodes = new List<ResourceNode>();
            foreach (var overlap in overlaps)
            {
                var node = overlap.GetComponent<ResourceNode>();
                if (node != null)
                    if (node.Type == ResourceType)
                    {
                        Nodes.Add(node);
                        node.Collectors.Add(this);
                    }
            }
        }

        public IEnumerator SpawnGatherer(int count)
        {
            var prefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/Friendlies/Gatherer");
            prefab.transform.position = transform.forward * -2.5f;

            for (var i = 0; i < count; i++)
            {
                var obj = Instantiate(prefab, transform);
                obj.GetComponent<NavMeshAgent>().enabled = true;
                var gatherer = obj.GetComponent<ResourceGatherer>();
                Gatherers.Add(gatherer);

                yield return new WaitForSeconds(1);

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
                if (!(dist < minDist)) continue;
                CurrentNode = node;
                minDist = dist;
            }
        }

        public void NotifyNodeDestroy(ResourceNode node)
        {
            Nodes.Remove(node);
            if (CurrentNode != node) return;
            GetClosestNode();
            foreach (var gatherer in Gatherers) gatherer.Node = CurrentNode;
        }

        public void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }

        private void SpawnNew()
        {
            if (Gatherers.Count >= 3) return;
            if (ResourceController.ResourceCount[ResourceTypes.Gold] <= 2) return;
            ResourceController.AddResource(ResourceTypes.Gold, -2);
            StartCoroutine(SpawnGatherer(1));
        }

        public override void Destroy()
        {
            foreach (var gatherer in Gatherers) Destroy(gatherer.gameObject);
            base.Destroy(true);
        }
    }
}