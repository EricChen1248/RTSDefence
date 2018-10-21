using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity_Components{
	public class GroupComponent : MonoBehaviour {

		// Use this for initialization
		public void Start () {
			
		}
		
		public bool Apply(GameObject Enemy){
			//true for agree false for decline
			return false;
		}
	}
}