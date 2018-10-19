using System.Collections;

namespace Entity_Components.Job
{
    public interface IJob
    {
        IEnumerator DoJob();
        void CancelJob();
    }
}
