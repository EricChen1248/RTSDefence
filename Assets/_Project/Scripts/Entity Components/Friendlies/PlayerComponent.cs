using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Jobs;
using Scripts.Entity_Components.Misc;
using Scripts.Interface;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Friendlies
{
    [DefaultExecutionOrder(-1)]
    public class PlayerComponent : MonoBehaviour, IClickable
    {
        public float Speed = 3.5f;
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator;

        public Job CurrentJob;
        public bool DoingJob;

        protected GameObject SelectionCirclePrefab;
        protected GameObject _selectionCircle;
        
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

            Agent.isStopped = false;
        }

        public void Stop()
        {
            Agent.isStopped = true;
        }


        #region Unity Callbacks

        public virtual void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.enabled = true;
            Animator = GetComponent<Animator>();
            GetComponent<HealthComponent>().OnDeath += (e) => Destroy(gameObject);
            var startLocation = Random.insideUnitSphere * 0.001f;
            startLocation.y = 0;
            MoveToLocation(transform.position + startLocation);

            SelectionCirclePrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Entities/SelectionCircle");
        }

        public IEnumerator CheckJob()
        {
            while(DoingJob || CurrentJob == null)
            {
                yield return new WaitForFixedUpdate();
            }
            DoingJob = true;
            StartCoroutine(CurrentJob.DoJob());
        }

        public void OnMouseUpAsButton()
        {
            CoreController.MouseController.SetFocus(this);
        }

        #endregion

        #region Focus

        public bool HasFocus { get; private set; }
        public virtual void Focus()
        {
            HasFocus = true;
            if(_selectionCircle == null)
            {
                _selectionCircle = Instantiate(SelectionCirclePrefab, transform);

                // Set y to 0.1 as not every model starts at the same location;
                var pos = _selectionCircle.transform.position;
                _selectionCircle.transform.position = new Vector3(pos.x, 0.1f, pos.z);
            }
        }

        public virtual void LostFocus()
        {
            HasFocus = false;
            if(_selectionCircle != null){
                Destroy(_selectionCircle);
                _selectionCircle = null;
            }
        }
        
        public virtual void RightClick()
        {
            Vector3 clickPos;
            if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

            MoveToLocationOnGrid(clickPos);
        }

        public void OnDestroy()
        {
            if (CoreController.MouseController.FocusedItem.Contains(this))
            {
                CoreController.MouseController.FocusedItem.Remove(this);
            }

        }

        #endregion
    }
}
