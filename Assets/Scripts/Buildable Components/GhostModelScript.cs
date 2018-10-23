using System;
using System.Collections.Generic;
using System.Net.Cache;
using Controllers;
using Entity_Components;
using Entity_Components.Friendlies;
using Entity_Components.Job;
using GUI;
using Helpers;
using Scriptable_Objects;
using UnityEngine;

namespace Buildable_Components
{
    /// <inheritdoc />
    /// <summary>
    /// Ghost model script that handles behaviors for ghost models.
    /// NOTE : Remember to disable colliders on prefabs
    /// </summary>
    public class GhostModelScript : MonoBehaviour
    {
        public BuildData Data { get; private set; }
        public List<RecipeItem> Recipe { get; private set; }
        public int WorkLeft { get; private set; }
        public PlayerComponent AssignedTo { get; private set; }
        public GameObject OriginalGameObject { get; private set; }

        public bool ActiveGhost;

        private void OnDisable()
        {
            ActiveGhost = false;
            GetComponent<BoxCollider>().enabled = false;
        }

        public BuildJob GenerateJob(PlayerComponent player)
        {
            AssignedTo = player;

            var job = new BuildJob(player, WorkLeft, this);
            return job;
        }

        public void Activate()
        {
            ActiveGhost = true;
            GetComponent<BoxCollider>().enabled = true;
        }

        public void AssignData(BuildData data, GameObject original)
        {
            Data = data;
            Recipe = new List<RecipeItem>(data.Recipe);
            WorkLeft = data.BuildTime;
            OriginalGameObject = original;
        }

        public void DoWork()
        {
            --WorkLeft;
        }

        public void JobCancel()
        {
            
        }

        public bool CanBuild()
        {
            return Physics.OverlapBox(transform.position, transform.localScale * 0.49f, transform.rotation).Length == 0;
        }

        public void Clicked()
        {
            if (!ActiveGhost) return;
            var ui = Pool.Spawn("FloatingUI");
            if (ui == null)
            {
                ui = Instantiate(Resources.Load<GameObject>("Prefabs/GUI/Floating UI"));
            }

            ui.transform.position = transform.position + Vector3.up;
            var texts = new List<string>{"B"};
            var jobs = new List<FloatingUIMenu.ClickEvent> {AssignJobToPlayer};
            ui.GetComponent<FloatingUIMenu>().AssignButton(texts, jobs);
        }

        private void AssignJobToPlayer()
        {
            var player = CoreController.MouseController._focusedItem as PlayerComponent;
            if (player == null) return;

            AssignedTo = player;
            var job = GenerateJob(player);
            player.CurrentJob = job;
            player.DoingJob = false;
        }
    }
}
