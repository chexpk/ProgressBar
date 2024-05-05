using System;
using System.Collections;
using DefaultNamespace.Progress.Settings;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressCounter : ITickable, IProgressCounter, IProgressCounterControl, IProgressCounterIncrease, IDisposable
    {
        public event Action<float> ProgressChanged;
        public float CurrentProgress { get; private set; } = 0;

        private ProgressSettings _progressSettings;
        // private bool _isProgressWork = true;
        private float _askedProgress = 0;
        private bool _isInProcess = false;

        //TODO ask Save/Load
        [Inject]
        public void Construct(ProgressSettings progressSettings)
        {
            _progressSettings = progressSettings;
        }

        public void Tick()
        {
            if (!_isInProcess)
            {
                return;
            }

            SmoothIncreaseProgress().MoveNext();
        }

        public void IncreaseProgress()
        {
            var normalizedProgress = NormalisedProgress();
            _askedProgress += normalizedProgress;

            if (!_isInProcess)
            {
                _isInProcess = true;
            }
        }

        public void SetProgressWork(bool isWork)
        {
            // _isProgressWork = isWork;
            _isInProcess = isWork;
        }

        private IEnumerator SmoothIncreaseProgress()
        {
            if (!_isInProcess)
            {
                yield break;
            }

            while (CurrentProgress < _askedProgress)
            {
                ProgressChanged?.Invoke(CurrentProgress);
                var value = _progressSettings.IncreaseSpeed * Time.deltaTime;
                CurrentProgress += value;
                yield return new WaitUntil( () => _isInProcess);
            }

            CurrentProgress = _askedProgress;
            ProgressChanged?.Invoke(CurrentProgress);
            _isInProcess = false;
        }

        private float NormalisedProgress()
        {
            return _progressSettings.IncreaseProgressButtonValue / _progressSettings.ProgressElementValue;
        }

        public void Dispose()
        {
            _isInProcess = false;
        }
    }
}