﻿using System.Collections;
using System.Collections.Generic;
using Scripts.Controllers;
using Scripts.Entity_Components.Friendlies;
using Scripts.Entity_Components.Job;
using Scripts.GUI;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Resources
{
    public class NodeManager : MonoBehaviour {

        public ResourceTypes ResourceType;

        public float HarvestTime;       // 一次花多少時間採集
        public float AvailableResource; // 有多少的量可採
        public float CollectionRadius;


        public List<ResourceCollectionJob> Collectors = new List<ResourceCollectionJob>();

        public void Clicked()
        {
            var ui = Pool.Spawn("FloatingUI");
            if (ui == null)
            {
                ui = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/GUI/Floating UI"));
            }

            ui.transform.position = transform.position + Vector3.up;
            var texts = new List<string> { "C" };
            var jobs = new List<FloatingUIMenu.ClickEvent> { AssignJobToPlayer };
            ui.GetComponent<FloatingUIMenu>().AssignButton(texts, jobs);
        }

        public float GatherResource()
        {
            --AvailableResource;

            if (!(AvailableResource <= 0)) return HarvestTime;

            foreach (var resourceCollectionJob in Collectors)
            {
                resourceCollectionJob.ResourcesGone();
            }

            StartCoroutine(WaitDestroy());

            return 0;

        }


        private void AssignJobToPlayer()
        {
            var player = CoreController.MouseController.FocusedItem as PlayerComponent;
            if (player == null) return;

            var job = GenerateJob(player);
            player.CurrentJob = job;
            player.DoingJob = false;
        }

        public ResourceCollectionJob GenerateJob(PlayerComponent player)
        {
            var job = new ResourceCollectionJob(player, this);
            return job;
        }

        private IEnumerator WaitDestroy()
        {
            yield return new WaitForFixedUpdate();
            Destroy(gameObject);
        }
    }
}