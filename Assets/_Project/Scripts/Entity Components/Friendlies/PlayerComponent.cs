using System;
using System.Collections;
using Scripts.Controllers;
using Scripts.Entity_Components.Jobs;
using Scripts.Interface;
using Scripts.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Friendlies
{
    [DefaultExecutionOrder(0)]
    public class PlayerComponent : MonoBehaviour, IClickable
    {
        public float Speed = 3.5f;
        public NavMeshAgent Agent { get; private set; }
        public Animator Animator;

        public Job CurrentJob;
        public bool DoingJob;

        public GameObject SelectionCirclePrefab;
        private GameObject _selectionCircle;
        
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

        public void OnMouseDown()
        {
            //CoreController.MouseController.SetFocus(this);
            CoreController.UnitSelectionController.SingleSelect = this;
        }

        #endregion

        #region Focus

        public bool HasFocus { get; private set; }
        public void Focus()
        {
            if(!HasFocus){
                gameObject.AddComponent<PathDrawer>();
                HasFocus = true;
                if(_selectionCircle == null){
                    _selectionCircle = Instantiate(SelectionCirclePrefab);
                    _selectionCircle.transform.SetParent(transform, false);
                }
            }
        }

        public void LostFocus()
        {
            if(HasFocus){
                Destroy(GetComponent<PathDrawer>());
                HasFocus = false;
                if(_selectionCircle != null){
                    Destroy(_selectionCircle);
                    _selectionCircle = null;
                }
            }
        }
        
        public void RightClick()
        {
            Vector3 clickPos;
            if (!RaycastHelper.TryMouseRaycastToGrid(out clickPos,
                RaycastHelper.LayerMaskDictionary["Walkable Surface"])) return;

            MoveToLocationOnGrid(clickPos);
        }
        
        #endregion
    }
}
