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
            JobController.FreeWorker(this);
            Agent.isStopped = false;
            Agent.SetDestination(CoreController.Instance.CoreGameObject.transform.position);
        }
    }
}