using System.Collections.Generic;
using Scripts.Buildable_Components;
using Scripts.Navigation;
using UnityEngine;

namespace Scripts.Resources
{
    public class ResourceNode : MonoBehaviour
    {
        public List<ResourceCollector> Collectors = new List<ResourceCollector>();
        public int count;
        public int PerCollection;
        public ResourceTypes Type;


        public Resource GetResource()
        {
            if (count-- == 0)
            {
                Destroy(gameObject);
                foreach (var collector in Collectors) collector.NotifyNodeDestroy(this);
                NavigationBaker.Instance.Rebuild();
            }

            return new Resource {Type = Type, Count = PerCollection};
        }
    }

    public struct Resource
    {
        public ResourceTypes Type;
        public int Count;
    }
}