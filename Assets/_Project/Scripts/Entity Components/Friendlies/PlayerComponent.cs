using System;
using Scripts.Entity_Components.Job;
using Scripts.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Friendlies
{
    [DefaultExecutionOrder(0)]
    public class PlayerComponent : MonoBehaviour, IPlayerControllable
    {
        public float Speed = 3.5f;
        public NavMeshAgent Agent { get; private set; }

        public IJob CurrentJob;
        public bool DoingJob;

        public void MoveToLocationOnGrid(Vector3 target)
        {
            target.x = Mathf.Round(target.x);
            target.y = Mathf.Round(target.y);
            target.z = Mathf.Round(target.z);

            MoveToLocation(target);
        }

        public void MoveToLocation(Vector3 target)
        {
            Agent.destination = target;
            Agent.isStopped = false;
        }

        public void Stop()
        {
            Agent.isStopped = true;
        }


        #region Unity Callbacks

        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.enabled = true;
        }
        
        private void FixedUpdate()
        {
            if (!DoingJob && CurrentJob != null)
            {
                DoingJob = true;
                StartCoroutine(CurrentJob.DoJob());   
            }
        }

        #endregion

        #region Focus

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            gameObject.AddComponent<PathDrawer>();
            HasFocus = true;
        }

        public void LostFocus()
        {
            Destroy(GetComponent<PathDrawer>());
            HasFocus = false;
        }
        
        public void RightClick(Vector3 clickPosition)
        {
            MoveToLocationOnGrid(clickPosition);
        }

        #endregion

    }
}
