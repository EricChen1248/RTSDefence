using System;
using System.Collections.Generic;
using Scripts.Scriptable_Objects;
using UnityEngine;

namespace Scripts.Entity_Components
{
    [DefaultExecutionOrder(-2)]
    public class GroupComponent : MonoBehaviour
    {
        private GroupDataProperty _groupProperty;
        public GroupData Data;

        //Member is modified by GroupFinder, not by GroupComponent itself.
        public HashSet<Transform> Member;

        public List<string> Characteristics()
        {
            return _groupProperty.Characteristics;
        }

        // Use this for initialization
        public void Start()
        {
            Member = new HashSet<Transform>();
            Data?.CompileGroupProperty(out _groupProperty);
        }

        public bool Apply(GameObject enemy)
        {
            //true for agree false for decline
            return true;
        }

        public void ApplyFunc<T>(Func<Transform, T> func)
        {
            //func cannot modify Member
            foreach (var member in Member) func(member);
        }

        public void ClearMember()
        {
            //make a copy MemberList so that KickedOut() can modify Member
            var memberList = new List<Transform>(Member);
            foreach (var member in memberList) member.GetComponent<GroupFinder>().KickedOut();
        }

        private void OnDestroy()
        {
            //ClearMember(); //only call from GroupFinder
        }

        public class GroupDataProperty
        {
            public List<string> Characteristics;
        }
    }
}