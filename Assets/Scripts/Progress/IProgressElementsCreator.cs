using System.Collections.Generic;

namespace DefaultNamespace.Progress
{
    public interface IProgressElementsCreator
    {
        IReadOnlyCollection<ProgressElement> ProgressElements { get; }
    }
}