using System;
using System.Collections.Generic;

namespace DefaultNamespace.Progress
{
    public class ProgressElementsCreator : IProgressElementsCreator
    {
        public IReadOnlyCollection<ProgressElement> ProgressElements
        {
            get
            {
                if (_progressElements == null)
                {
                    _progressElements = CreateProgressElements();
                }

                return _progressElements;
            }
        }

        private IReadOnlyCollection<ProgressElement> _progressElements;

        //TODO check settings
        private IReadOnlyCollection<ProgressElement> CreateProgressElements()
        {
            ProgressElement[] progressElements = new ProgressElement[8];
            var currentTime = DateTime.UtcNow;
            for (int i = 0; i < 8; i++)
            {
                progressElements[i] = new ProgressElement(false, false, currentTime, 0, i);
            }

            return progressElements;
        }


    }
}