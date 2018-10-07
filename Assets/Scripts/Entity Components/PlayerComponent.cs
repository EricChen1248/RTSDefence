﻿using Controllers;
using Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components
{
    [DefaultExecutionOrder(0)]
    public class PlayerComponent : MonoBehaviour, IClickable
    {
        public float Speed = 3.5f;
        private NavMeshAgent _agent;

        // Use this for initialization
        private void Start ()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = true;
        }
    
        public void MoveToLocation(Vector3 target)
        {
            target.x = Mathf.Round(target.x);
            target.y = Mathf.Round(target.y);
            target.z = Mathf.Round(target.z);

            _agent.destination = target;
            _agent.isStopped = false;
        }

        private void OnMouseDown()
        {
            CoreController.MouseController.SetFocus(this);
        }
    

        private static int IndexFromMask(int mask)
        {
            for (var i = 0; i < 32; ++i)
            {
                if (1 << i == mask)
                    return i;
            }
            return -1;
        }

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
    }
}