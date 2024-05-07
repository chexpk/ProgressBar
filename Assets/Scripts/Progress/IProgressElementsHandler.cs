using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DefaultNamespace.Progress
{
    public interface IProgressElementsHandler
    {
        IReadOnlyCollection<ProgressElement> ProgressElements { get; }
        event Action<int> ChangedElementIndex;
        Task<bool> LoadComplication();
    }
}