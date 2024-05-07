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

        public T CreateDefault<T>() where T : Data, new()
        {
            if (typeof(T) == typeof(ElementsData))
            {
                return CreateElementsData() as T;
            }

            if (typeof(T) == typeof(CounterData))
            {
                return CreateCounterData() as T;
            }

            return default;
        }

        private ElementsData CreateElementsData()
        {
            return new ElementsData()
            {
                ProgressElements = CreateProgressElements()
            };
        }

        private CounterData CreateCounterData()
        {
            return new CounterData()
            {
                Progress = 0,
                AskedProgress = 0
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