using Scripts.Controllers;
using Scripts.Entity_Components.Ais;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class GroupFinder : MonoBehaviour
    {
        private static Transform _groupsPool; //point to "Groups" which is parent of all group
        private Transform _group;

        public Transform Group
        {
            get { return _group; }
            private set
            {
                if (_group != null) KickedOut();

                _group = value;
                if (_group == null) return;

                value.GetComponent<GroupAiBase>().FirstCommand(transform);
                var groupComponent = value.GetComponent<GroupComponent>();
                groupComponent.Member.Add(transform);
            }
        }

        public void Start()
        {
            // Is this a good way?
            if (_groupsPool == null) _groupsPool = CoreController.Instance.GroupsGameObject.transform;
            Search();
        }

        //de-register
        public void KickedOut(bool selfDestroy = false)
        {
            if (_group == null) return;
            var groupComponent = _group.GetComponent<GroupComponent>();
            groupComponent.Member?.Remove(transform);
            _group.GetComponent<GroupAiBase>().LastCommand(transform, selfDestroy);
            _group = null;

            if (selfDestroy) return;
            GetComponent<SingularAiBase>().FindTarget();
            Search();
        }

        //search for an existing group
        public void Search()
        {
            if (_groupsPool != null)
                foreach (var group in _groupsPool.GetComponent<GroupManager>().Children())
                {
                    var success = group.GetComponent<GroupComponent>().Apply(gameObject);
                    if (!success) continue;
                    Group = group;
                    return;
                }

            // if failed:
            SearchFailed();
        }

        private void SearchFailed()
        {
            print("No commander... I can only Uraaaa!");
            GetComponent<SingularAiBase>().FindTarget();
            //this.enabled = false;
        }

        private void OnDestroy()
        {
            KickedOut(true);
        }
    }
}