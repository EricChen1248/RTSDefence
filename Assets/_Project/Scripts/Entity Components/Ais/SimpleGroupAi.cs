using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais{
	[DefaultExecutionOrder(-1)]
	public class SimpleGroupAi : GroupAiBase {
		//random for test
		public static System.Random rnd = new System.Random();
		private static int PosXLowerBound = -10;
		private static int PosXUpperBound = 10;
		private static int PosZLowerBound = -10;
		private static int PosZUpperBound = 10;

		public override void FindTarget(){}
		public override void LeadMemberTo(Vector3 Vector){
			foreach(Transform member in _groupData.Member){
				member.GetComponent<NavMeshAgent>().destination = Vector + new Vector3(rnd.Next(PosXLowerBound, PosXUpperBound), 0, rnd.Next(PosZLowerBound, PosZUpperBound));
			}
		}
	}
}