using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity_Components{
	public class GroupFinder : MonoBehaviour {
		private bool _active;
		private GameObject Group;
		// Use this for initialization
		public void Start () {
			_active = GetComponent<EnemyComponent>().FindGroup;
			if(_active){
				Search();
			}
		}
		
		//search for an existing group
		public void Search () {
			
		}
	}
}