using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Entity_Components;

namespace Entity_Components.Ais{
	[DefaultExecutionOrder(-1)]
	public class SimpleGroupAi : AiBase {
		private GroupComponent _groupData;
		// Use this for initialization
		void Start () {
			_groupData = GetComponent<GroupComponent>();
		}
		// Update is called once per frame
		void Update () {
			
		}
		public override void FindTarget(){}
	}
}