using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Components.Ais{
	public class SimpleGroupAi : MonoBehaviour {
		private bool _active;
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