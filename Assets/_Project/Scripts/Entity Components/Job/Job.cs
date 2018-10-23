using System.Collections;

namespace Scripts.Entity_Components.Job
{
    public interface IJob
    {
        IEnumerator DoJob();
        void CancelJob();
    }
}
