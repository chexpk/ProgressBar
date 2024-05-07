using System;
using DefaultNamespace.Progress;
using Zenject;

namespace DefaultNamespace.Reward
{
    public class Rewarder : IInitializable, IDisposable
    {
        private readonly IProgressElementsHandler _progressElementsHandler;

        public Rewarder(IProgressElementsHandler progressElementsHandler)
        {
            _progressElementsHandler = progressElementsHandler;
        }

        public async void Initialize()
        {
            await _progressElementsHandler.LoadComplication();
            Init();
        }

        private async void Init()
        {
            var task = _progressElementsHandler.LoadComplication();
            await task;
            if (!task.IsCompleted)
            {
                return;
            }

            foreach (var element in _progressElementsHandler.ProgressElements)
            {
                if (element.IsReceived)
                {
                    continue;
                }

                if (!element.IsAchieved)
                {
                    element.Achieved += OnAchieved;
                    continue;
                }

                var timer = new ElementsReceiveTimer(element.TimeOfAchieved, element.DelayToReceived, element.SetTimeToReceived, element.SetReceived);
                timer.Launch();
            }
        }

        private void OnAchieved(ProgressElement element)
        {
            element.Achieved -= OnAchieved;
            var timer = new ElementsReceiveTimer(element.TimeOfAchieved, element.DelayToReceived, element.SetTimeToReceived, element.SetReceived);
            timer.Launch();
        }

        public void Dispose()
        {
            foreach (var element in _progressElementsHandler.ProgressElements)
            {
                element.Achieved -= OnAchieved;
            }
        }
    }
}