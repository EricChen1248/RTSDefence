using UnityEngine;

namespace Scripts.Entity_Components.Ais
{
    public abstract class SingularAiBase : AiBase
    {
        protected bool StopTemp;
        protected GameObject TempTarget;

        public abstract void StopTempTarget();

        public bool HasTempTarget()
        {
            return TempTarget != null;
        }
    }
}