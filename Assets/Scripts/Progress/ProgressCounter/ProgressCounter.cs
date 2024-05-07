using System;
using System.Collections;
using DefaultNamespace.Progress.Settings;
using DefaultNamespace.System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressCounter : ITickable, IProgressCounter, IProgressCounterIncrease, ISaving, IInitializable, IDisposable
    {
        private const string ProfileName = "CounterData";
        public event Action<float> ProgressChanged;

        private readonly ProgressSettings _progressSettings;
        private readonly ISaver _saver;
        private readonly ISaveSystem _saveSystem;
        private CounterData _counterData;
        private float _currentProgress = 0;
        private float _askedProgress = 0;
        private bool _isInProcess = false;

        public ProgressCounter(ProgressSettings progressSettings, ISaver saver, ISaveSystem saveSystem)
        {
            _progressSettings = progressSettings;
            _saver = saver;
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
            _saver.RegisterToSave(this);
            LoadSaveData();
        }

        public void Dispose()
        {
            _isInProcess = false;
            _saver.Unregister(this);
        }

        private void LoadSaveData()
        {
            _counterData = _saveSystem.Load<CounterData>(ProfileName);
            _askedProgress = _counterData.AskedProgress;
            _currentProgress = _counterData.Progress;
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

        public void SelfSave()
        {
            CounterData dataToSave = new CounterData()
            {
                AskedProgress = _askedProgress, Progress = _currentProgress
            };

            _saveSystem.Save(ProfileName, dataToSave);
        }

        private IEnumerator SmoothIncreaseProgress()
        {
            if (!_isInProcess)
            {
                yield break;
            }

            while (_currentProgress < _askedProgress)
            {
                ProgressChanged?.Invoke(_currentProgress);
                var value = _progressSettings.IncreaseSpeed * Time.deltaTime;
                _currentProgress += value;
                yield return new WaitUntil( () => _isInProcess);
            }

            _currentProgress = _askedProgress;
            ProgressChanged?.Invoke(_currentProgress);
            _isInProcess = false;
        }

        private float NormalisedProgress()
        {
            return _progressSettings.IncreaseProgressButtonValue / _progressSettings.ProgressElementValue;
        }
    }
}