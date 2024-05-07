using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ElementsReceiveTimer
    {
        private UniTaskCompletionSource<bool> _uniTaskCompletionSource;
        private readonly DateTime _achievedTime;
        private float _delay;
        private Action<float> _onTimeChanged;
        private Action _onTimeEnd;
        private float _timer = 0;

        public ElementsReceiveTimer(DateTime achievedTime, float delay, Action<float> onTimeChanged, Action onTimeEnd)
        {
            _achievedTime = achievedTime;
            _delay = delay;
            _onTimeChanged = onTimeChanged;
            _onTimeEnd = onTimeEnd;
        }

        public async UniTaskVoid Launch()
        {
            var diff = (int)((DateTime.UtcNow - _achievedTime).TotalSeconds);
            _delay = Mathf.Max(0, _delay - diff);

            bool result = await LaunchTimer();
            if (!result)
            {
                Debug.LogAssertion("Timer not finished");
                return;
            }

            _onTimeEnd?.Invoke();
            _onTimeEnd = null;
        }

        private async UniTask<bool> LaunchTimer()
        {
            if (_uniTaskCompletionSource != null && !_uniTaskCompletionSource.Task.Status.IsCompleted())
            {
                return await _uniTaskCompletionSource.Task;
            }

            _uniTaskCompletionSource = new UniTaskCompletionSource<bool>();
            while (_timer < _delay)
            {
                _timer += Time.deltaTime;
                SetLeftTime(_timer);
                await Task.Yield();
            }

            _timer = 0;
            _onTimeChanged = null;
            _uniTaskCompletionSource?.TrySetResult(true);
            return true;
        }

        private void SetLeftTime(float timer)
        {
            var leftTime = Mathf.Max(0, _delay - timer);
            _onTimeChanged?.Invoke(leftTime);
        }
    }
}