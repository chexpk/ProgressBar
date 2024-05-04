using System;
using System.Collections.Generic;

namespace DefaultNamespace.Progress
{
    public interface IProgressElementsHandler
    {
        IReadOnlyCollection<ProgressElement> ProgressElements { get; }
        event Action<int> ChangedElementIndex;
    }
}