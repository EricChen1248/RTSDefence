using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entity_Components
{
    [DefaultExecutionOrder(-2)]
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

        public void ApplyFunc<T>(System.Func<Transform, T> func)
        {
            //func cannot modify Member
            foreach (var member in Member)
            {
                func(member);
            }
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
            //ClearMember(); //only call from GroupFinder
        }
    }
}