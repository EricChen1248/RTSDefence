using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity_Components.Ais;
using Controllers;

namespace Entity_Components{
	public class GroupFinder : MonoBehaviour {
		private Transform _group;
		private static Transform _groupsPool; //point to "Groups" which is parent of all group
		// Use this for initialization
		public void Start () {
			// Is this a good way?
			if(_groupsPool == null){
				_groupsPool = CoreController.Instance.GroupsGameObject?.transform;
			}
			Search();
		}

		public void KickedOut(){
			_group = null;
		}
		
		//search for an existing group
		public void Search () {
			int? NumOfGroups = _groupsPool?.childCount;
			if(NumOfGroups != null){
				for(int i = 0;i < NumOfGroups;i++){
					print("Apply for Group No." + i.ToString());
					Transform Group = _groupsPool.GetChild(i);
					bool Success = Group.GetComponent<GroupComponent>().Apply(this.gameObject);
					if(Success){
						_group = Group;
						//_group.?? += KickedOut;
						return;
					}
				}
			}
			// if failed:
			SearchFailed();
		}

		private void SearchFailed(){
			print("No commander... I can only Uraaaa!");
			GetComponent<AiBase>().FindTarget();
			//this.enabled = false;
		}
	}
}