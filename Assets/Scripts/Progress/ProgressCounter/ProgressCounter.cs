using System;
using System.Collections;
using DefaultNamespace.Progress.Settings;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressCounter : MonoBehaviour, IProgressCounter, IProgressCounterPause, IProgressCounterIncrease
    {
        public event Action<float> ProgressChanged;
        public float CurrentProgress { get; private set; } = 0;
        public bool IsProgressWork { get; set; } = true;

        private float _askedProgress = 0;
        private ProgressSettings _progressSettings;
        private bool _isInProcess = false;
        private Coroutine _coroutine;

        //TODO ask Save/Load
        [Inject]
        public void Construct(ProgressSettings progressSettings)
        {
            _progressSettings = progressSettings;
        }

        public void IncreaseProgress()
        {
            var normalizedProgress = NormalisedProgress();
            _askedProgress += normalizedProgress;

            if (!_isInProcess)
            {
                _coroutine = StartCoroutine(SmoothIncreaseProgress());
            }
        }

        private IEnumerator SmoothIncreaseProgress()
        {
            _isInProcess = true;

            while (CurrentProgress < _askedProgress)
            {
                var value = _progressSettings.IncreaseSpeed * Time.deltaTime;
                CurrentProgress += value;
                if (CurrentProgress < _askedProgress)
                {
                    ProgressChanged?.Invoke(CurrentProgress);
                }

                yield return new WaitUntil( (() => IsProgressWork));
                yield return null;
            }

            CurrentProgress = _askedProgress;
            ProgressChanged?.Invoke(CurrentProgress);
            _isInProcess = false;
        }

        private float NormalisedProgress()
        {
            return _progressSettings.IncreaseProgressButtonValue / _progressSettings.ProgressElementValue;
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}