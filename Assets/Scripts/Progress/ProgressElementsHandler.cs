using System;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class ProgressElementsHandler : IProgressElementsHandler, IDisposable
    {
        public ProgressElement[] ProgressElements
        {
            get
            {
                if (_progressElements == null)
                {
                    _progressElements = _progressElementsCreator.ProgressElements;
                }

                return _progressElements;
            }
        }

        private readonly IProgressElementsCreator _progressElementsCreator;
        private readonly IProgressCounter _progressCounter;
        private ProgressElement[] _progressElements;
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

        //TODO проскок через несколько элекментов
        private void SetLocalProgress(float progress)
        {
            int index = (int)Mathf.Floor(progress);
            if (TrySetMaxProgress(index))
            {
                _progressElements[_currentElementIndex].SetMaxProgress();
                return;
            }

            if (_currentElementIndex < index)
            {
                _progressElements[_currentElementIndex].SetMaxProgress();
                _currentElementIndex = index;
            }

            _progressElements[_currentElementIndex].SetSelfProgress(progress - index);
        }

        private bool TrySetMaxProgress(int index)
        {
            if(index < _progressElements.Length)
            {
                return false;
            }

            return _isMaxProgress = true;
        }

        public void Dispose()
        {
            _progressCounter.ProgressChanged += OnProgressChanged;
        }
    }
}