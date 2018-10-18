using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity_Components.Ais;

namespace Entity_Components{
	public class GroupFinder : MonoBehaviour {
		private GameObject _group;
		// Use this for initialization
		public void Start () {
			Search();
		}

		public void KickedOut(){
			_group = null;
		}
		
		//search for an existing group
		public void Search () {
			_group = this.gameObject; //test
			//_group.?? += KickedOut;
			// if failed:
			SearchFailed();
		}

		public void SearchFailed(){
			print("No commander... I can only Uraaaa!");
			GetComponent<AiBase>().FindTarget();
			//this.enabled = false;
		}
	}
}