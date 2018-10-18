using System.Collections;

namespace Entity_Components.Job
{
    public interface IJob
    {
        IEnumerator DoJob(PlayerComponent sender);
        IEnumerator ResumeJob(PlayerComponent sender);
    }
}
