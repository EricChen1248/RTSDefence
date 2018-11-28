using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.Interface;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.VR;

namespace Scripts.Buildable_Components
{
    public class ResourceCollector : Buildable, IClickable 
    {
        public int CollectionRadius;
        public List<ResourceNode> Nodes;
        private GameObject _range;

        public override void Start()
        {
            base.Start();
            GetResourceInRange();
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
                    Nodes.Add(node);
                }

            }
        }

        private void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            HasFocus = true;
            _range = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Map Objects/RangeShower"), transform);
            _range.transform.localScale = new Vector3(CollectionRadius, 5, CollectionRadius);
        }

        public void LostFocus()
        {
            HasFocus = false;
            Destroy(_range);
        }

        public void RightClick(Vector3 clickPosition)
        {

        }
    }
}
