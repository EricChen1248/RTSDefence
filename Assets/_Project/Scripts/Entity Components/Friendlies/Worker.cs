using System;
using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Jobs;
using UnityEngine;

namespace Scripts.Entity_Components.Friendlies
{
    public class Worker : PlayerComponent
    {
        public override void Start()
        {
            base.Start();
            CompleteJob();
        }

        public void DoWork(Job job)
        {
            job.Worker = this;
            StartCoroutine(job.DoJob());
        }
        
        public override void RightClick(Vector3 click)
        {

        }

        public void CompleteJob()
        {
            var job = JobController.GetJob();

            if (job == null)
            {
                JobController.FreeWorker(this);

                // Return to core
                Agent.destination = CoreController.Instance.CoreGameObject.transform.position;
                return;
            }

            DoWork(job);
        }


    }
}
