using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.System;

namespace DefaultNamespace.Progress
{
    public class DefaultDataCreator : IDefaultDataCreator
    {

        private readonly DefaultDataSettings _defaultDataSettings;

        public DefaultDataCreator(DefaultDataSettings defaultDataSettings)
        {
            _defaultDataSettings = defaultDataSettings;
        }

        public SaveData DefaultSaveData()
        {
            return new SaveData()
            {
                ProgressElementsData = new ProgressElementsData()
                {
                    ProgressElements = CreateProgressElements(),
                    CurrnetElementIndex = 0,
                    IsMaxProgress = false
                },

                ProgressCounterData = new ProgressCounterData()
                {
                    AskedProgress = 0, Progress = 0,
                }
            };
        }

        private List<ProgressElement> CreateProgressElements()
        {
            ProgressElement[] progressElements = new ProgressElement[_defaultDataSettings.CountOfProgressElements];
            var currentTime = DateTime.UtcNow;
            for (int i = 0; i < _defaultDataSettings.CountOfProgressElements; i++)
            {
                progressElements[i] = new ProgressElement(false, false, currentTime, 0, i, _defaultDataSettings.DelayToReceived);
            }

            return progressElements.ToList();
        }


    }
}