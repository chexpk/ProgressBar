using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressElementsHandler : IProgressElementsHandler, IDisposable, ISaving, IInitializable
    {
        public event Action<int> ChangedElementIndex;
        public IReadOnlyCollection<ProgressElement> ProgressElements => _progressElements;

        private readonly IProgressCounter _progressCounter;
        private readonly ISaver _saver;
        private IReadOnlyCollection<ProgressElement> _progressElements;
        private SaveData _saveData;
        private int _currentElementIndex = 0;
        private bool _isMaxProgress = false;

        public ProgressElementsHandler(IProgressCounter progressCounter, ISaver saver)
        {
            _progressCounter = progressCounter;
            _saver = saver;

            _progressCounter.ProgressChanged += OnProgressChanged;
            _saver.RegisterToSave(this);
        }

        public void Initialize()
        {
            LoadSaveData(_saver);
            LaunchElementRewardTimers();
        }

        public void SelfSave(SaveData saveData)
        {
            List<ProgressElement> progressElements = new();
            foreach (var element in _progressElements)
            {
                progressElements.Add(element);
            }

            saveData.ProgressElementsData = new ProgressElementsData()
            {
                ProgressElements = progressElements,
                // CurrentElementIndex = _currentElementIndex,
                // IsMaxProgress = _isMaxProgress
            };
        }

        private void LoadSaveData(ISaver saver)
        {
            _saveData = saver.LoadSaveData();
            _progressElements = _saveData.ProgressElementsData.ProgressElements;
            // _currentElementIndex = _saveData.ProgressElementsData.CurrentElementIndex;
            // _isMaxProgress = _saveData.ProgressElementsData.IsMaxProgress;
        }

        private void OnProgressChanged(float progress)
        {
            if (_isMaxProgress)
            {
                return;
            }

            SetLocalProgress(progress);
        }

        private void SetLocalProgress(float progress)
        {
            int index = (int)Mathf.Floor(progress);
            if (TrySetMaxProgress(index))
            {
                MaxElementProgressFrom(_currentElementIndex);
                ChangedElementIndex?.Invoke(_progressElements.Count - 1);
                return;
            }

            if (_currentElementIndex < index)
            {
                for (int i = _currentElementIndex; i < index; i++)
                {
                    SetMaxForElementByIndex(i);
                }

                _currentElementIndex = index;
                ChangedElementIndex?.Invoke(_currentElementIndex);
            }

            _progressElements.ElementAt(_currentElementIndex).SetSelfProgress(progress - index);
        }

        private void MaxElementProgressFrom(int index)
        {
            for (int i = index; i < _progressElements.Count; i++)
            {
                SetMaxForElementByIndex(i);
            }
        }

        private bool TrySetMaxProgress(int index)
        {
            if(index < _progressElements.Count)
            {
                return false;
            }

            return _isMaxProgress = true;
        }

        private void SetMaxForElementByIndex(int index)
        {
            if (_progressElements.ElementAt(index).IsAchieved)
            {
                return;
            }

            _progressElements.ElementAt(index).SetMaxProgress(DateTime.UtcNow);
        }

        private void LaunchElementRewardTimers()
        {
            foreach (var element in _progressElements)
            {
                if (element.IsAchieved && !element.IsReceived)
                {
                    element.StartReceiveTimer();
                }
            }
        }

        public void Dispose()
        {
            _progressCounter.ProgressChanged -= OnProgressChanged;
            _saver.Unregister(this);
        }
    }
}