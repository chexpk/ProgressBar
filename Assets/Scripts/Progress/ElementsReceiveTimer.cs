using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ElementsReceiveTimer
    {
        private ProgressElement _progressElement;
        private DateTime _achievedTime;

        private TaskCompletionSource<bool> _completionSource;
        private float _timer = 0;
        private float _delay = 5;

        public ElementsReceiveTimer(ProgressElement progressElement)
        {
            _progressElement = progressElement;
        }

        public Task<bool> Launch()
        {
            _achievedTime = _progressElement.TimeOfAchieved;
            var diff = (float)(_achievedTime - DateTime.UtcNow).Seconds;
            _delay -= diff;
            if (_delay < 0)
            {
                _delay = 0;
            }

            return LaunchTimer();
        }

        private Task<bool> LaunchTimer()
        {
            if (_completionSource != null && !_completionSource.Task.IsCanceled)
            {
                return _completionSource.Task;
            }

            _completionSource = new TaskCompletionSource<bool>();
            Timer();
            return _completionSource.Task;
        }

        private async UniTaskVoid Timer()
        {
            while (_timer < _delay)
            {
                _timer += Time.deltaTime;
                SetLeftTime(_timer);
                await Task.Yield();
            }

            _timer = 0;
            _completionSource?.TrySetResult(true);
            _progressElement = null;
        }

        private void SetLeftTime(float timer)
        {
            var leftTime = _delay - timer;
            if (leftTime < 0)
            {
                leftTime = 0;
            }

            _progressElement.SetTimeToReceived(leftTime);
        }
    }
}