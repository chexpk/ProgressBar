using System;
using System.Collections;
using DefaultNamespace.Progress.Settings;
using DefaultNamespace.System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressCounter : ITickable, IProgressCounter, IProgressCounterControl, IProgressCounterIncrease, IDisposable, ISaving, IInitializable
    {
        public event Action<float> ProgressChanged;
        public float CurrentProgress { get; private set; } = 0;

        private readonly ProgressSettings _progressSettings;
        private readonly ISaver _saver;
        private ProgressCounterData _progressCounterData;

        private float _askedProgress = 0;
        private bool _isInProcess = false;

        //TODO ask Save/Load
        public ProgressCounter(ProgressSettings progressSettings, ISaver saver)
        {
            _progressSettings = progressSettings;
            _saver = saver;

            _saver.RegisterToSave(this);
        }

        public void Initialize()
        {
            _progressCounterData = _saver.ProgressCounterData();
            _askedProgress = _progressCounterData.AskedProgress;
            CurrentProgress = _progressCounterData.Progress;
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
            _isInProcess = isWork;
        }

        public void SelfSave(SaveData saveData)
        {
            saveData.ProgressCounterData = new ProgressCounterData()
            {
                AskedProgress = _askedProgress, Progress = CurrentProgress
            };
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
            _saver.Unregister(this);
        }
    }
}