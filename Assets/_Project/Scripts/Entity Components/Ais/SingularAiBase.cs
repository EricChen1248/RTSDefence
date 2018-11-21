using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Entity_Components.Ais
{
    public abstract class SingularAiBase : AiBase
    {
        protected GameObject _tempTarget;
        protected bool _stopTemp;

        public abstract void StopTempTarget();
    }
}
