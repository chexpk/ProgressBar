using System;

namespace DefaultNamespace.Progress
{
    public interface IProgressElementsHandler
    {
        ProgressElement[] ProgressElements { get; }
        event Action<int> ChangedElementIndex;
    }
}