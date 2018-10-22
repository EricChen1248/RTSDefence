using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Entity_Components;

namespace Entity_Components.Ais{
	[RequireComponent(typeof(GroupComponent))]
	public abstract class GroupAiBase : AiBase {
		protected GroupComponent _groupData;

		// Use this for initialization
		protected void Start () {
			_groupData = GetComponent<GroupComponent>();
		}

		public abstract void LeadMemberTo(Vector3 Vector);
	}
}