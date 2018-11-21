using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

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
        public override void FindTarget(){
            SwitchState(new StopState(transform));
            //if find:
            //TargetTo(target.gameObject, force: true);
        }
        public override bool TargetTo(GameObject obj, bool force){
            if (!InLayerMask(TargetingLayers, obj.layer) && !force){
                return false;
            }
            SwitchState(new TargetingState(transform, obj.transform));
            return true;
        }
        public override void FirstCommand(Transform member){
            _state?.FirstCommand(member);
        }
        public override void LastCommand(Transform member, bool selfDestroy){
            _state?.LastCommand(member, selfDestroy);
            //member.GetComponent<SingularAiBase>().FindTarget(); //This will be done in GroupFinder
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
        private void OnDestroy(){
            _state?.Leave();
            _state = null;
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
            public override void LastCommand(Transform member, bool selfDestroy){
                if(!selfDestroy){
                    member.GetComponent<NavMeshAgent>().isStopped = false;
                }
            }
        }

        protected class SelfDecideState : State{
            public SelfDecideState(Transform t) : base(t){}
            public override String Identifier{get{return "SelfDecide";}}
            public override void Enter(){
                var group_data = _transform.GetComponent<GroupComponent>();
                foreach (var member in group_data.Member)
                {
                    member.GetComponent<SingularAiBase>().FindTarget();
                }
            }
            public override void Update(){}
            public override void Leave(){}
            public override void FirstCommand(Transform member){}
            public override void LastCommand(Transform member, bool selfDestroy){}
        }

        protected class MoveState : State{
            //random for test
            public static System.Random Rnd = new System.Random();
            private const int PosXLowerBound = -10;
            private const int PosXUpperBound = 10;
            private const int PosZLowerBound = -10;
            private const int PosZUpperBound = 10;

            protected const int _step0bond = 10;
            protected const int _step1bond = 8;
            protected Vector3 _vector{
                get{return _transform.position;}
                set{_transform.position = value;}
            }
            protected readonly GroupComponent _groupComponent;
            protected byte _step;
            public byte Step{
                get{return _step;}
            }
            protected HashSet<Transform> _notStopped;
            private List<Transform> _toBeRemoved;
            public MoveState(Transform t, Vector3 vector) : base(t){
                _vector = vector;
                _groupComponent = _transform.GetComponent<GroupComponent>();
                _step = 0;
                _notStopped = new HashSet<Transform>();
                _toBeRemoved = new List<Transform>();
            }
            public override String Identifier{get{return "Move";}}
            public override void Enter(){
                foreach (var member in _groupComponent.Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = _vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0, Rnd.Next(PosZLowerBound, PosZUpperBound));
                }
            }
            /*
            Q: Performance good??
            A: Use _notStopped to reduce the demand on detecting the distances of all members every frame.
            */
            public override void Update(){
                if(_step == 0){
                    foreach (var member in _groupComponent.Member)
                    {
                        if(Vector3.Distance(member.position, member.GetComponent<NavMeshAgent>().destination) < _step0bond){
                            _step = 1;
                        }
                        break;
                    }
                    if(_step == 1){
                        print("set correct place");
                        foreach (var member in _groupComponent.Member){
                            if(Vector3.Distance(member.position, _vector) < _step1bond){
                                member.GetComponent<NavMeshAgent>().ResetPath();
                            }else{
                                member.GetComponent<NavMeshAgent>().destination = _vector;
                                _notStopped.Add(member);
                            }
                        }
                    }
                }else{
                    foreach (var member in _notStopped)
                    {
                        if(Vector3.Distance(member.position, _vector) < _step1bond){
                            //_transform.GetComponent<SimpleGroupAi>().StopAll();
                            member.GetComponent<NavMeshAgent>().ResetPath();
                            _toBeRemoved.Add(member);
                        }
                    }
                    foreach(var member in _toBeRemoved){
                        print("Move Stop " + member.GetInstanceID());
                        _notStopped.Remove(member);
                    }
                    _toBeRemoved.Clear();
                }
            }
            public override void Leave(){
                foreach (var member in _groupComponent.Member)
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
                    _notStopped.Add(member);
                }
            }
            public override void LastCommand(Transform member, bool selfDestroy){
                _notStopped?.Remove(member);
            }
        }

        protected class TargetingState : State{
            public Transform Target;
            public TargetingState(Transform t, Transform target) : base(t){
                Target = target; //This one will be usually used.
            }
            public override String Identifier{get{return "Targeting";}}
            public override void Enter(){
                _transform.GetComponent<GroupAiBase>().Target = Target.gameObject; //This one is just to match AiBase.
                foreach (var member in _transform.GetComponent<GroupComponent>().Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = Target.position;
                }
                Target.GetComponent<HealthComponent>().OnDeath += OnTargetDeath;
            }
            public override void Update(){}
            public override void Leave(){
                _transform.GetComponent<GroupAiBase>().Target = null;
                Target.GetComponent<HealthComponent>().OnDeath -= OnTargetDeath;
            }
            public override void FirstCommand(Transform member){
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                agent.destination = Target.position;
            }
            public override void LastCommand(Transform member, bool selfDestroy){}
            protected void OnTargetDeath(HealthComponent th){
                //FindTarget will switch state and therefore Leave will be called. Unsubscribing will happen.
                //In C# it is safe for the method to unsubscribe the event when the event is invoking the method.
                _transform.GetComponent<GroupAiBase>().FindTarget();
            }
        }
    }
}