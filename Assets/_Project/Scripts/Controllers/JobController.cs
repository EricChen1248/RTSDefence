using System;
using System.Collections.Generic;
using Scripts.Entity_Components.Friendlies;
using Scripts.Entity_Components.Jobs;
using UnityEngine;

namespace Scripts.Controllers
{
    [DefaultExecutionOrder(-999)]
    public class JobController : MonoBehaviour
    {
        private static JobController _instance;
        private LinkedList<Job> _jobQueue;

        private Queue<Worker> _workers;

        // Use this for initialization
        private void Start()
        {
            _jobQueue = new LinkedList<Job>();
            _workers = new Queue<Worker>();
            _instance = this;
        }

        private void FixedUpdate()
        {
            if (_workers.Count <= 0 || _jobQueue.Count <= 0) return;
            var worker = _workers.Dequeue();
            var job = _jobQueue.First.Value;
            _jobQueue.RemoveFirst();

            worker.DoWork(job);
        }

        public static void FreeWorker(Worker worker)
        {
            _instance._workers.Enqueue(worker);
        }

        public static void AddJob(Job job)
        {
            _instance.AddJobInstance(job);
        }

        public static void Prioritize(Job job)
        {
            _instance._jobQueue.Remove(job);
            _instance._jobQueue.AddFirst(job);
        }

        public static void CancelJob(Job job)
        {
            _instance._jobQueue.Remove(job);
        }

        private void AddJobInstance(Job job)
        {
            _jobQueue.AddLast(job);

            if (_workers.Count > 0)
            {
                var worker = _workers.Dequeue();
                worker.CompleteJob();
            }
        }

        private Job GetJobInstance()
        {
            if (_jobQueue.Count <= 0) return null;

            var job = _jobQueue.First.Value;
            _jobQueue.RemoveFirst();

            return job;
        }
    }
}