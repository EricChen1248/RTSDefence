using Scripts.Entity_Components.Friendlies;
using Scripts.Entity_Components.Jobs;
using Scripts.Scriptable_Objects;
using Scripts.Controllers;
using Scripts.Navigation;
using Scripts.Helpers;
using Scripts.GUI;

using System.Collections.Generic;
using Scripts.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Buildable_Components
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
        public GameObject OriginalGameObject { get; private set; }

        public bool ActiveGhost;
        private Dictionary<ResourceTypes, int> _droppedResources;

        private BuildJob _job;

        public void Activate()
        {
            ActiveGhost = true;
            GetComponent<BoxCollider>().enabled = true;
            _droppedResources = new Dictionary<ResourceTypes, int>();

            // Generate new build job for ghost
            _job = new BuildJob(this);
            JobController.AddJob(_job);
        }

        public void DepositResources(ResourceTypes type, int count)
        {
            if (!_droppedResources.ContainsKey(type))
            {
                _droppedResources[type] = 0;
            }

            _droppedResources[type] += count;
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

        public bool CanBuild()
        {
            var overlap = Physics.OverlapBox(transform.position + Data.ColliderOffset, Data.Size / 2f * 0.9f, transform.rotation, RaycastHelper.LayerMaskDictionary["Non Buildables"]);
            foreach (var item in overlap)
            {
                return false;
            }
            return true;
        }

        public void Clicked()
        {
            if (!ActiveGhost) return;
            var omg = ObjectMenuGroupComponent.Instance;

            omg.ResetButtons();
            omg.SetButton(0, "Cancel", Cancel);
            omg.SetButton(1, "Prioritize", Prioritize);
            omg.Show();
        }

        public void Cancel()
        {
            foreach (var droppedResource in _droppedResources)
            {
                ResourceController.AddResource(droppedResource.Key, droppedResource.Value);
            }
            JobController.CancelJob(_job);
        }

        public void Prioritize()
        {
            JobController.Prioritize(_job);
        }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            if (other.gameObject == _job.Worker.gameObject)
            {
                _job.AtDestination = true;
            }
        }
    }
}
