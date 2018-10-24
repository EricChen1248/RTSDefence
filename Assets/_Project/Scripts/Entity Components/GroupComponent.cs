using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity_Components
{
    public class GroupComponent : MonoBehaviour
    {
        //Member is modified by GroupFinder, not by GroupComponent itself.
        public HashSet<Transform> Member;

        // Use this for initialization
        public void Start()
        {
            Member = new HashSet<Transform>();
        }

        public bool Apply(GameObject enemy)
        {
            //true for agree false for decline
            return true;
        }

        public void ClearMember()
        {
            //make a copy MemberList so that KickedOut() can modify Member
            var memberList = new List<Transform>(Member);
            foreach (var member in memberList)
            {
                member.GetComponent<GroupFinder>().KickedOut();
            }
        }

        private void OnDestroy()
        {
            ClearMember();
        }
    }
}