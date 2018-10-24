using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleGroupAi : GroupAiBase
    {
        //random for test
        public static System.Random Rnd = new System.Random();
        private const int PosXLowerBound = -10;
        private const int PosXUpperBound = 10;
        private const int PosZLowerBound = -10;
        private const int PosZUpperBound = 10;

        public override void FindTarget() { }
        public override void FirstCommand(Transform member) { }
        public override void LastCommand(Transform member) { }
        public override void LeadMemberTo(Vector3 vector)
        {
            foreach (var member in GroupData.Member)
            {
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                agent.destination = vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0, Rnd.Next(PosZLowerBound, PosZUpperBound));
            }
        }
    }
}