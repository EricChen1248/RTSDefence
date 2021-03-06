using UnityEngine;

namespace Scripts.Entity_Components.Ais
{
    [RequireComponent(typeof(GroupComponent))]
    public abstract class GroupAiBase : AiBase
    {
        protected GroupAIProperty AIProperty;

        protected GroupComponent GroupComponent;

        // Use this for initialization
        public override void Start()
        {
            GroupComponent = GetComponent<GroupComponent>();
            GroupComponent.Data?.CompileAIProperty(out AIProperty);
        }

        //No matter if an enemy would find a group or not,
        //it first calls its AI to find a target.
        //If it has a group, then when it joins,
        //the AI of the group may send a command
        //to make it stop, to have a party for its initiation, or whatever.
        //This method gets called when an enemy set its group.
        //"Member" of group component does not contain this member.
        public abstract void FirstCommand(Transform member);

        //And that is what needs to be done when a member gets kicked out.
        //Maybe it will be useful when a group perishes.
        //"Member" of group component does not contain this member.
        public abstract void LastCommand(Transform member, bool selfDestroy);
        public abstract void StopAll();

        public abstract void LeadMemberTo(Vector3 vector);

        //Provide a prototype of states.
        //Implementation will be in subclasses.
        public abstract class State
        {
            protected readonly Transform _transform;

            public State(Transform t)
            {
                _transform = t;
            }

            public abstract string Identifier { get; }
            public abstract void Enter();
            public abstract void Update();
            public abstract void Leave();
            public abstract void FirstCommand(Transform member);
            public abstract void LastCommand(Transform member, bool selfDestroy);
        }

        public class GroupAIProperty
        {
            public bool CheckTempTarget;
            public float TimeToClearTempTarget;
        }
    }
}