using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity_Components{
	public class GroupComponent : MonoBehaviour {
		//Member is modified by GroupFinder, not by GroupComponent itself.
		public HashSet<Transform> Member;

		// Use this for initialization
		public void Start () {
			Member = new HashSet<Transform>();
		}
		
		public bool Apply(GameObject Enemy){
			//true for agree false for decline
			return true;
		}

		public void ClearMember(){
			//make a copy MemberList so that KickedOut() can modify Member
			var MemberList = new List<Transform>(Member);
			foreach(Transform member in MemberList){
				member.GetComponent<GroupFinder>().KickedOut();
			}
		}

		void OnDestroy(){
			ClearMember();
		}
	}
}