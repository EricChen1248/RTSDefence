using UnityEngine;
using UnityEngine.AI;
using System;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleGroupAi : GroupAiBase
    {
        protected State _state;

        protected new void Start(){
            base.Start();
            _state = null;
            SwitchState(new SelfDecideState(transform));
        }
        protected void SwitchState(State state){
            _state?.Leave();
            print(state.Identifier);
            _state = state;
            _state?.Enter();
        }
        public override void FindTarget() { }
        public override void FirstCommand(Transform member){
            _state?.FirstCommand(member);
        }
        public override void LastCommand(Transform member){
            _state?.LastCommand(member);
            //member.GetComponent<AiBase>().FindTarget(); //This will be done in GroupFinder
        }
        public override void StopAll(){
            SwitchState(new StopState(transform));
        }
        public override void LeadMemberTo(Vector3 vector)
        {
            SwitchState(new MoveState(transform, vector));
        }
        protected void Update(){
            _state?.Update();
        }

        //Define All Kinds of States
        protected class StopState : State{
            public StopState(Transform t) : base(t){}
            public override String Identifier{get{return "Stop";}}
            public override void Enter(){
                var group_data = _transform.GetComponent<GroupComponent>();
                foreach (var member in group_data.Member)
                {
                    member.GetComponent<NavMeshAgent>().isStopped = true;
                }
            }
            public override void Update(){}
            public override void Leave(){
                var group_data = _transform.GetComponent<GroupComponent>();
                foreach (var member in group_data.Member)
                {
                    member.GetComponent<NavMeshAgent>().isStopped = false;
                }
            }
            public override void FirstCommand(Transform member){
                member.GetComponent<NavMeshAgent>().isStopped = true;
            }
            public override void LastCommand(Transform member){
                member.GetComponent<NavMeshAgent>().isStopped = false;
            }
        }

        protected class SelfDecideState : State{
            public SelfDecideState(Transform t) : base(t){}
            public override String Identifier{get{return "SelfDecide";}}
            public override void Enter(){
                var group_data = _transform.GetComponent<GroupComponent>();
                foreach (var member in group_data.Member)
                {
                    member.GetComponent<AiBase>().FindTarget();
                }
            }
            public override void Update(){}
            public override void Leave(){}
            public override void FirstCommand(Transform member){}
            public override void LastCommand(Transform member){}
        }

        protected class MoveState : State{
            //random for test
            public static System.Random Rnd = new System.Random();
            private const int PosXLowerBound = -10;
            private const int PosXUpperBound = 10;
            private const int PosZLowerBound = -10;
            private const int PosZUpperBound = 10;

            protected readonly Vector3 _vector;
            protected readonly GroupComponent GroupData;
            protected int _step;
            public MoveState(Transform t, Vector3 vector) : base(t){
                _vector = vector;
                GroupData = _transform.GetComponent<GroupComponent>();
                _step = 0;
            }
            public override String Identifier{get{return "Move";}}
            public override void Enter(){
                foreach (var member in GroupData.Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = _vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0, Rnd.Next(PosZLowerBound, PosZUpperBound));
                }
            }
            public override void Update(){
                if(_step == 0){
                    foreach (var member in GroupData.Member)
                    {
                        if(Vector3.Distance(member.position, member.GetComponent<NavMeshAgent>().destination) < 1){
                            _step = 1;
                        }
                        break;
                    }
                    if(_step == 1){
                        print("set correct place");
                        foreach (var member in GroupData.Member){
                            member.GetComponent<NavMeshAgent>().destination = _vector;
                        }
                    }
                }
            }
            public override void Leave(){
                foreach (var member in GroupData.Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.ResetPath();
                }
            }
            public override void FirstCommand(Transform member){
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                if(_step == 0){
                    agent.destination = _vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0, Rnd.Next(PosZLowerBound, PosZUpperBound));
                }else{
                    agent.destination = _vector;
                }
            }
            public override void LastCommand(Transform member){}
        }

        protected class TargetingState : State{
            public TargetingState(Transform t) : base(t){}
            public override String Identifier{get{return "Targeting";}}
            public override void Enter(){}
            public override void Update(){}
            public override void Leave(){}
            public override void FirstCommand(Transform member){}
            public override void LastCommand(Transform member){}
        }
    }
}