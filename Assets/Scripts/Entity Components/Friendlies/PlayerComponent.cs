using Controllers;
using Entity_Components.Job;
using Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Friendlies
{
    [DefaultExecutionOrder(0)]
    public class PlayerComponent : MonoBehaviour, IPlayerControllable
    {
        public float Speed = 3.5f;
        public NavMeshAgent Agent { get; private set; }

        public IJob CurrentJob;
        public bool DoingJob;

        public void MoveToLocation(Vector3 target)
        {
            target.x = Mathf.Round(target.x);
            target.y = Mathf.Round(target.y);
            target.z = Mathf.Round(target.z);

            Agent.destination = target;
            Agent.isStopped = false;
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
                StartCoroutine(CurrentJob.DoJob());   
            }
        }

        #endregion

        #region Focus

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            HasFocus = true;
        }

        public void LostFocus()
        {
            HasFocus = false;
        }
        
        public void RightClick(Vector3 clickPosition)
        {
            MoveToLocation(clickPosition);
        }

        #endregion

    }
}
