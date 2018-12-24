using System.Collections;
using Scripts.Entity_Components.Friendlies;

namespace Scripts.Entity_Components.Jobs
{
    public abstract class Job
    {
        public Worker Worker;
        public abstract IEnumerator DoJob();
        public abstract void CancelJob();

        public void CompleteJob()
        {
            Worker.CompleteJob();
        }
    }
}