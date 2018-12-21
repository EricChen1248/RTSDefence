using UnityEngine;

namespace Scripts.Entity_Components.Ais
{
    public abstract class SingularAiBase : AiBase
    {
        protected GameObject TempTarget;
        protected bool StopTemp;

        public abstract void StopTempTarget();

        public bool HasTempTarget()
        {
            return TempTarget != null;
        }
    }
}
