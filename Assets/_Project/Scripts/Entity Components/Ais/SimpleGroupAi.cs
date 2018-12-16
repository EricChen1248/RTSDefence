using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Entity_Components.Misc;

namespace Scripts.Entity_Components.Ais
{
    [DefaultExecutionOrder(-1)]
    public class SimpleGroupAi : GroupAiBase
    {
        protected State _State;
        protected HashSet<SingularAiBase> HasTempTarget;

        protected new void Start()
        {
            base.Start();
            _State = null;
            HasTempTarget = new HashSet<SingularAiBase>();
            SwitchState(new SelfDecideState(transform));
        }

        protected void SwitchState(State state)
        {
            _State?.Leave();
            print(state.Identifier);
            _State = state;
            _State?.Enter();
        }

        public override void FindTarget()
        {
            SwitchState(new StopState(transform));
            //if find:
            //TargetTo(target.gameObject, force: true);
        }

        public override bool TargetTo(GameObject obj, bool force)
        {
            if (!InLayerMask(TargetingLayers, obj.layer) && !force)
            {
                return false;
            }

            SwitchState(new TargetingState(transform, obj.transform));
            return true;
        }

        public override void FirstCommand(Transform member)
        {
            _State?.FirstCommand(member);
        }

        public override void LastCommand(Transform member, bool selfDestroy)
        {
            _State?.LastCommand(member, selfDestroy);
            //member.GetComponent<SingularAiBase>().FindTarget(); //This will be done in GroupFinder
        }

        public override void StopAll()
        {
            SwitchState(new StopState(transform));
        }

        public override void LeadMemberTo(Vector3 vector)
        {
            SwitchState(new MoveState(transform, vector));
        }

        protected void Update()
        {
            _State?.Update();
            CommonUpdate();
        }

        protected void CommonUpdate()
        {
            if (!AIProperty.CheckTempTarget) return;

            foreach (var member in GroupComponent.Member)
            {
                var singularAI = member.GetComponent<SingularAiBase>();
                if (!singularAI.HasTempTarget() || HasTempTarget.Contains(singularAI)) continue;

                HasTempTarget.Add(singularAI);
                StartCoroutine(StopMemberTempTarget(singularAI));
            }
        }

        protected IEnumerator StopMemberTempTarget(SingularAiBase singularAI)
        {
            yield return new WaitForSeconds(AIProperty.TimeToClearTempTarget);
            singularAI.StopTempTarget();
            HasTempTarget.Remove(singularAI);
        }

        private void OnDestroy()
        {
            _State?.Leave();
            _State = null;
        }

        //Define All Kinds of States
        protected class StopState : State
        {
            public StopState(Transform t) : base(t)
            {
            }

            public override string Identifier => "Stop";

            public override void Enter()
            {
                var groupData = _transform.GetComponent<GroupComponent>();
                foreach (var member in groupData.Member)
                {
                    member.GetComponent<NavMeshAgent>().isStopped = true;
                }
            }

            public override void Update()
            {
            }

            public override void Leave()
            {
                var groupData = _transform.GetComponent<GroupComponent>();
                foreach (var member in groupData.Member)
                {
                    member.GetComponent<NavMeshAgent>().isStopped = false;
                }
            }

            public override void FirstCommand(Transform member)
            {
                member.GetComponent<NavMeshAgent>().isStopped = true;
            }

            public override void LastCommand(Transform member, bool selfDestroy)
            {
                if (!selfDestroy)
                {
                    member.GetComponent<NavMeshAgent>().isStopped = false;
                }
            }
        }

        protected class SelfDecideState : State
        {
            public SelfDecideState(Transform t) : base(t)
            {
            }

            public override string Identifier => "SelfDecide";

            public override void Enter()
            {
                // TODO disabled group ai

                //var groupData = _transform.GetComponent<GroupComponent>();
                //foreach (var member in groupData.Member)
                //{
                //    member.GetComponent<SingularAiBase>().FindTarget();
                //}
            }

            public override void Update() { }
            public override void Leave() { }
            public override void FirstCommand(Transform member) { }
            public override void LastCommand(Transform member, bool selfDestroy) { }
        }

        protected class MoveState : State
        {
            //random for test
            public static System.Random Rnd = new System.Random();
            private const int PosXLowerBound = -10;
            private const int PosXUpperBound = 10;
            private const int PosZLowerBound = -10;
            private const int PosZUpperBound = 10;

            protected const int Step0Bond = 10;
            protected const int Step1Bond = 8;

            protected Vector3 Vector
            {
                get { return _transform.position; }
                set { _transform.position = value; }
            }

            protected readonly GroupComponent GroupComponent;
            protected byte _step;

            public byte Step => _step;

            protected HashSet<Transform> NotStopped;
            private readonly List<Transform> _toBeRemoved;

            public MoveState(Transform t, Vector3 vector) : base(t)
            {
                Vector = vector;
                GroupComponent = _transform.GetComponent<GroupComponent>();
                _step = 0;
                NotStopped = new HashSet<Transform>();
                _toBeRemoved = new List<Transform>();
            }

            public override string Identifier => "Move";

            public override void Enter()
            {
                foreach (var member in GroupComponent.Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = Vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0,
                                            Rnd.Next(PosZLowerBound, PosZUpperBound));
                }
            }

            /*
            Q: Performance good??
            A: Use _notStopped to reduce the demand on detecting the distances of all members every frame.
            */
            public override void Update()
            {
                if (_step == 0)
                {
                    foreach (var member in GroupComponent.Member)
                    {
                        if (Vector3.Distance(member.position, member.GetComponent<NavMeshAgent>().destination) <
                            Step0Bond)
                        {
                            _step = 1;
                        }

                        break;
                    }

                    if (_step != 1) return;
                    {
                        print("set correct place");
                        foreach (var member in GroupComponent.Member)
                        {
                            if (Vector3.Distance(member.position, Vector) < Step1Bond)
                            {
                                member.GetComponent<NavMeshAgent>().ResetPath();
                            }
                            else
                            {
                                member.GetComponent<NavMeshAgent>().destination = Vector;
                                NotStopped.Add(member);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var member in NotStopped)
                    {
                        if (!(Vector3.Distance(member.position, Vector) < Step1Bond)) continue;
                        //_transform.GetComponent<SimpleGroupAi>().StopAll();
                        member.GetComponent<NavMeshAgent>().ResetPath();
                        _toBeRemoved.Add(member);
                    }

                    foreach (var member in _toBeRemoved)
                    {
                        print("Move Stop " + member.GetInstanceID());
                        NotStopped.Remove(member);
                    }

                    _toBeRemoved.Clear();
                }
            }

            public override void Leave()
            {
                foreach (var member in GroupComponent.Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.ResetPath();
                }
            }

            public override void FirstCommand(Transform member)
            {
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                if (_step == 0)
                {
                    agent.destination = Vector + new Vector3(Rnd.Next(PosXLowerBound, PosXUpperBound), 0,
                                            Rnd.Next(PosZLowerBound, PosZUpperBound));
                }
                else
                {
                    agent.destination = Vector;
                    NotStopped.Add(member);
                }
            }

            public override void LastCommand(Transform member, bool selfDestroy)
            {
                NotStopped?.Remove(member);
            }
        }

        protected class TargetingState : State
        {
            public Transform Target;

            public TargetingState(Transform t, Transform target) : base(t)
            {
                Target = target; //This one will be usually used.
            }

            public override string Identifier => "Targeting";

            public override void Enter()
            {
                _transform.GetComponent<GroupAiBase>().Target = Target.gameObject; //This one is just to match AiBase.
                foreach (var member in _transform.GetComponent<GroupComponent>().Member)
                {
                    var agent = member.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = Target.position;
                }

                Target.GetComponent<HealthComponent>().OnDeath += OnTargetDeath;
            }

            public override void Update() { }

            public override void Leave()
            {
                _transform.GetComponent<GroupAiBase>().Target = null;
                Target.GetComponent<HealthComponent>().OnDeath -= OnTargetDeath;
            }

            public override void FirstCommand(Transform member)
            {
                var agent = member.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                agent.destination = Target.position;
            }

            public override void LastCommand(Transform member, bool selfDestroy) { }

            protected void OnTargetDeath(HealthComponent th)
            {
                //FindTarget will switch state and therefore Leave will be called. Unsubscribing will happen.
                //In C# it is safe for the method to unsubscribe the event when the event is invoking the method.
                _transform.GetComponent<GroupAiBase>().FindTarget();
            }
        }
    }
}