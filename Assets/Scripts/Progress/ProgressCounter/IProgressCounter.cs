using System;

namespace DefaultNamespace.Progress
{
    public interface IProgressCounter
    {
        event Action<float> ProgressChanged;
        void SetProgressWork(bool isWork);
    }
}