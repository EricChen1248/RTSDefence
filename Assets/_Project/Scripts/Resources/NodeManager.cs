using System.Collections;
using System.Collections.Generic;
using Scripts.Entity_Components.Jobs;
using UnityEngine;

namespace Scripts.Resources
{
    public class NodeManager : MonoBehaviour
    {
        public float AvailableResource; // 有多少的量可採
        public float CollectionRadius;

        public List<ResourceCollectionJob> Collectors = new List<ResourceCollectionJob>();

        public float HarvestTime; // 一次花多少時間採集

        public ResourceTypes ResourceType;


        public float GatherResource()
        {
            --AvailableResource;

            if (!(AvailableResource <= 0)) return HarvestTime;

            foreach (var resourceCollectionJob in Collectors) resourceCollectionJob.ResourcesGone();

            StartCoroutine(WaitDestroy());

            return 0;
        }

        private IEnumerator WaitDestroy()
        {
            yield return new WaitForFixedUpdate();
            Destroy(gameObject);
        }
    }
}