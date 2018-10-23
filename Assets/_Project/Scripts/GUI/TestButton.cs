using Scripts.Controllers;
using Scripts.Entity_Components.Ais;
using UnityEngine;

namespace Scripts.GUI{
	//write a method in TestButton
	//and make ClickEvent call the method
	//Free to add properties, but for compatibility, not to remove properties or methods
	public class TestButton : MonoBehaviour {
		public Vector3 V;
		public void ClickEvent(){
			//call one method
			LeadAllGroup();
		}

		public void LeadAllGroup(){
			foreach(Transform Group in CoreController.Instance.GroupsGameObject.GetComponent<GroupsComponent>().Children()){
				Group.GetComponent<GroupAiBase>().LeadMemberTo(V);
			}
		}
		//Add some methods if you want...
	}
}