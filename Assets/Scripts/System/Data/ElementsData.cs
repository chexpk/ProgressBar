using System;
using System.Collections.Generic;
using DefaultNamespace.Progress;

namespace DefaultNamespace.System
{
    [Serializable]
    public record ElementsData : Data
    {
        public List<ProgressElement> ProgressElements;
    }
}