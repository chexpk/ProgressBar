using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ElementsReceiveTimer
    {
        private UniTaskCompletionSource<bool> _utcs;
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
            _delay -= diff;
            if (_delay < 0)
            {
                _delay = 0;
            }

            bool result = await LaunchTimer();
            if (!result)
            {
                Debug.Log("Timer not finished");
                return;
            }

            _onTimeEnd?.Invoke();
            _onTimeEnd = null;
        }

        private async UniTask<bool> LaunchTimer()
        {
            if (_utcs != null && !_utcs.Task.Status.IsCompleted())
            {
                return await _utcs.Task;
            }

            _utcs = new UniTaskCompletionSource<bool>();
            while (_timer < _delay)
            {
                _timer += Time.deltaTime;
                SetLeftTime(_timer);
                await Task.Yield();
            }

            _timer = 0;
            _onTimeChanged = null;
            _utcs?.TrySetResult(true);
            return true;
        }

        private void SetLeftTime(float timer)
        {
            var leftTime = _delay - timer;
            if (leftTime < 0)
            {
                leftTime = 0;
            }

            _onTimeChanged?.Invoke(leftTime);
        }
    }
}