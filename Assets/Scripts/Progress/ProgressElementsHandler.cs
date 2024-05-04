using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ProgressElementsHandler : IProgressElementsHandler, IDisposable
    {
        public event Action<int> ChangedElementIndex;
        public IReadOnlyCollection<ProgressElement> ProgressElements => _progressElements ??= _progressElementsCreator.ProgressElements;

        private readonly IProgressElementsCreator _progressElementsCreator;
        private readonly IProgressCounter _progressCounter;
        private IReadOnlyCollection<ProgressElement> _progressElements;
        private int _currentElementIndex = 0;

        //TODO need correct load and connect to CurrentProgress in _progressCounter
        private bool _isMaxProgress = false;

        public ProgressElementsHandler(IProgressElementsCreator progressElementsCreator, IProgressCounter progressCounter)
        {
            _progressElementsCreator = progressElementsCreator;
            _progressCounter = progressCounter;

            _progressCounter.ProgressChanged += OnProgressChanged;
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
                MaxProgress();
                ChangedElementIndex?.Invoke(_progressElements.Count - 1);
                return;
            }

            if (_currentElementIndex < index)
            {
                for (int i = _currentElementIndex; i < index; i++)
                {
                    _progressElements.ElementAt(i).SetMaxProgress(DateTime.UtcNow);
                }

                _currentElementIndex = index;
                ChangedElementIndex?.Invoke(_currentElementIndex);
            }

            _progressElements.ElementAt(_currentElementIndex).SetSelfProgress(progress - index);
        }

        private void MaxProgress()
        {
            for (int i = _currentElementIndex; i < _progressElements.Count; i++)
            {
                _progressElements.ElementAt(i).SetMaxProgress(DateTime.UtcNow);
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

        public void Dispose()
        {
            _progressCounter.ProgressChanged -= OnProgressChanged;
        }
    }
}