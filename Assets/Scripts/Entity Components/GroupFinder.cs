using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

		//register
		public void SetGroup(Transform Group){
			if(_group != null){
				KickedOut();
			}
			_group = Group;
			if(_group == null){
				return;
			}
			var group_component = Group.GetComponent<GroupComponent>();
			print(group_component);
			group_component.Member.Add(transform);
		}

		//de-register
		public void KickedOut(){
			if(_group == null){
				return;
			}
			var group_component = _group.GetComponent<GroupComponent>();
			group_component.Member.Remove(transform);
			_group = null;
		}
		
		//search for an existing group
		public void Search () {
			if(_groupsPool != null){
				foreach(Transform Group in _groupsPool.GetComponent<GroupsComponent>().Children()){
					bool Success = Group.GetComponent<GroupComponent>().Apply(this.gameObject);
					if(Success){
						SetGroup(Group);
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