using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleGroupAi : GroupAiBase
    {
        protected enum State{Stop, Moving, Targeting, SelfDecide};
        protected struct StateData{
            public Vector3 Place;
        }
        protected State _state;
        //random for test
        public static System.Random Rnd = new System.Random();
        private const int PosXLowerBound = -10;
        private const int PosXUpperBound = 10;
        private const int PosZLowerBound = -10;
        private const int PosZUpperBound = 10;

        protected new void Start(){
            base.Start();
            _state = State.SelfDecide;
        }
        public override void FindTarget() { }
        public override void FirstCommand(Transform member){
            var agent = member.GetComponent<NavMeshAgent>();
            switch(_state){
                case State.Stop:
                    //stop at its place
                    agent.isStopped = true;
                    break;
                case State.Moving:
                    break;
            }
        }
        public override void LastCommand(Transform member) { }
        public override void StopAll(){
            foreach (var member in GroupData.Member)
            {
                member.GetComponent<NavMeshAgent>().isStopped = true;
            }
            _state = State.Stop;
        }
        public override void LeadMemberTo(Vector3 vector)
        {
            foreach (var member in GroupData.Member)
            {
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                agent.destination = vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0, Rnd.Next(PosZLowerBound, PosZUpperBound));
            }
        }
        protected void Update(){
            switch(_state){
                case State.Stop:
                    break;
                default:
                    break;
            }
        }
    }
}