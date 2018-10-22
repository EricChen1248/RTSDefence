using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Entity_Components;
using Entity_Components.Ais;

namespace GUI{
	public class TestButton : MonoBehaviour {
		public Vector3 V;
		public void ClickEvent(){
			//do everything you want to do.
			foreach(Transform Group in CoreController.Instance.GroupsGameObject.GetComponent<GroupsComponent>().Children()){
				Group.GetComponent<GroupAiBase>().LeadMemberTo(V);
			}
		}
	}
}