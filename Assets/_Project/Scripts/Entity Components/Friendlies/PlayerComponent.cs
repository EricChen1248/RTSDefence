using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Jobs;
using Scripts.Entity_Components.Misc;
using Scripts.Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Friendlies
{
    [DefaultExecutionOrder(-1)]
    public class PlayerComponent : MonoBehaviour, IClickable
    {
        protected string _type = "player";
        public Animator Animator;

        public Job CurrentJob;

        protected IEnumerator DestinationRoutine;
        public bool DoingJob;

        public int ResourceCount;
        protected GameObject SelectionCircle;

        protected GameObject SelectionCirclePrefab;
        public float Speed = 3.5f;
        public NavMeshAgent Agent { get; private set; }
        public string Type => _type;

        public void MoveToLocationOnGrid(Vector3 target)
        {
            target.x = Mathf.Round(target.x);
            target.y = Mathf.Round(target.y);
            target.z = Mathf.Round(target.z);

            MoveToLocation(target);
        }

        public virtual void MoveToLocation(Vector3 target)
        {
            Agent.destination = target;
            Animator.SetBool("Walking", true);
            Agent.isStopped = false;

            if (DestinationRoutine != null) StopCoroutine(DestinationRoutine);
            DestinationRoutine = AtDestination();
            StartCoroutine(DestinationRoutine);
        }

        public void Stop()
        {
            Animator.SetBool("Walking", false);
            Agent.isStopped = true;
        }


        #region Unity Callbacks

        public virtual void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.enabled = true;
            Animator = GetComponent<Animator>();
            GetComponent<HealthComponent>().OnDeath += e => Destroy(gameObject);
            var startLocation = Random.insideUnitSphere * 0.001f;
            startLocation.y = 0;
            MoveToLocation(transform.position + startLocation);

            SelectionCirclePrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/SelectionCircle");
            Stop();
        }

        public IEnumerator CheckJob()
        {
            while (DoingJob || CurrentJob == null) yield return new WaitForFixedUpdate();
            DoingJob = true;
            StartCoroutine(CurrentJob.DoJob());
        }

        public void OnMouseDown()
        {
            CoreController.UnitSelectionController.SingleSelect = this;
        }

        #endregion

        #region Focus

        public bool HasFocus { get; private set; }

        public virtual void Focus()
        {
            HasFocus = true;
            gameObject.AddComponent<PathDrawer>();
            if (SelectionCircle == null)
            {
                SelectionCircle = Instantiate(SelectionCirclePrefab, transform);

                // Set y to 0.1 as not every model starts at the same location;
                var pos = SelectionCircle.transform.position;
                SelectionCircle.transform.position = new Vector3(pos.x, 0.1f, pos.z);
            }
        }

        public virtual void LostFocus()
        {
            Destroy(GetComponent<PathDrawer>());
            HasFocus = false;
            if (SelectionCircle != null)
            {
                Destroy(SelectionCircle);
                SelectionCircle = null;
            }
        }

        public virtual void RightClick(Vector3 clickPos)
        {
            MoveToLocation(clickPos);
        }

        public void OnDestroy()
        {
            if (CoreController.MouseController.FocusedItem.Contains(this))
                CoreController.MouseController.FocusedItem.Remove(this);
        }

        public IEnumerator AtDestination()
        {
            while ((transform.position - Agent.destination).sqrMagnitude > 2f) yield return new WaitForFixedUpdate();
            Stop();
            DestinationRoutine = null;
        }

        #endregion
    }
}