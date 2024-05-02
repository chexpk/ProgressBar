using System;

namespace DefaultNamespace.Progress
{
    public interface IProgressCounter
    {
        event Action<float> ProgressChanged;
        float CurrentProgress { get; }
    }
}