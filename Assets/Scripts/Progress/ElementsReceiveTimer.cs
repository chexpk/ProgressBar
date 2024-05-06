using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ElementsReceiveTimer
    {
        private TaskCompletionSource<bool> _completionSource;
        private readonly DateTime _achievedTime;
        private float _delay;
        private Action<float> _onTimeChanged;
        private float _timer = 0;

        public ElementsReceiveTimer(DateTime achievedTime, float delay, Action<float> onTimeChanged)
        {
            _achievedTime = achievedTime;
            _delay = delay;
            _onTimeChanged = onTimeChanged;
        }

        public Task<bool> Launch()
        {
            var diff = (int)((DateTime.UtcNow - _achievedTime).TotalSeconds);
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
            _onTimeChanged = null;
        }

        private void SetLeftTime(float timer)
        {
            var leftTime = _delay - timer;
            if (leftTime < 0)
            {
                leftTime = 0;
            }

            _onTimeChanged(leftTime);
        }
    }
}