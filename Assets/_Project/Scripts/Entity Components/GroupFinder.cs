﻿using Scripts.Controllers;
using Scripts.Entity_Components.Ais;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class GroupFinder : MonoBehaviour
    {
        private Transform _group;
        private static Transform _groupsPool; //point to "Groups" which is parent of all group
                                              // Use this for initialization
        public void Start()
        {
            // Is this a good way?
            if (_groupsPool == null)
            {
                _groupsPool = CoreController.Instance.GroupsGameObject?.transform;
            }
            Search();
        }

        //register
        public void SetGroup(Transform group)
        {
            if (_group != null)
            {
                KickedOut();
            }
            _group = group;
            if (_group == null) return;

            var groupComponent = group.GetComponent<GroupComponent>();
            groupComponent.Member.Add(transform);
            group.GetComponent<GroupAiBase>().FirstCommand(transform);
        }

        //de-register
        public void KickedOut()
        {
            if (_group == null)
            {
                return;
            }
            var groupComponent = _group.GetComponent<GroupComponent>();
            groupComponent.Member.Remove(transform);
            _group.GetComponent<GroupAiBase>().LastCommand(transform);
            _group = null;
        }

        //search for an existing group
        public void Search()
        {
            if (_groupsPool != null)
            {
                foreach (var @group in _groupsPool.GetComponent<GroupManager>().Children())
                {
                    var success = @group.GetComponent<GroupComponent>().Apply(gameObject);
                    if (!success) continue;
                    SetGroup(@group);
                    return;
                }
            }
            // if failed:
            SearchFailed();
        }

        private void SearchFailed()
        {
            print("No commander... I can only Uraaaa!");
            GetComponent<AiBase>().FindTarget();
            //this.enabled = false;
        }

        private void OnDestroy()
        {
            KickedOut();
        }
    }
}