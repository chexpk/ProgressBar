using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace.System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Progress
{
    public class ProgressElementsHandler : IProgressElementsHandler, IDisposable, ISaving, IInitializable
    {
        private const string ProfileName = "ElementsData";
        public event Action<int> ChangedElementIndex;
        public IReadOnlyCollection<ProgressElement> ProgressElements => _progressElements;

        private readonly IProgressCounter _progressCounter;
        private readonly ISaver _saver;
        private readonly ISaveSystem _saveSystem;
        private ElementsData _elementsData;
        private IReadOnlyCollection<ProgressElement> _progressElements;
        private readonly TaskCompletionSource<bool> _loadCompletionSource;
        private int _currentElementIndex = 0;
        private bool _isMaxProgress = false;

        public ProgressElementsHandler(
            IProgressCounter progressCounter,
            ISaver saver,
            ISaveSystem saveSystem)
        {
            _progressCounter = progressCounter;
            _saver = saver;
            _saveSystem = saveSystem;

            _progressCounter.ProgressChanged += OnProgressChanged;
            _loadCompletionSource = new TaskCompletionSource<bool>();
        }

        public void Initialize()
        {
            _saver.RegisterToSave(this);
            LoadSaveData();
        }

        public Task<bool> LoadComplication()
        {
            return _loadCompletionSource.Task;
        }

        public void SelfSave()
        {
            List<ProgressElement> progressElements = new();
            foreach (var element in _progressElements)
            {
                progressElements.Add(element);
            }

            ElementsData dataToSave = new ElementsData()
            {
                ProgressElements = progressElements
            };

            _saveSystem.Save(ProfileName, dataToSave);
        }

        private void LoadSaveData()
        {
            _elementsData = _saveSystem.Load<ElementsData>(ProfileName);
            _progressElements = _elementsData.ProgressElements;
            _loadCompletionSource?.TrySetResult(true);
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

        public void Dispose()
        {
            _progressCounter.ProgressChanged -= OnProgressChanged;
            _saver.Unregister(this);
            _loadCompletionSource?.TrySetResult(false);
        }
    }
}