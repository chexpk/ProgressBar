using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.System
{
    public class SaveObserver : MonoBehaviour,  ISaver
    {
        private ISaveSystem _saveSystem;
        private SaveData _currentSaveData;
        private readonly List<ISaving> _savings = new();

        [Inject]
        public void Construct(ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public void RegisterToSave(ISaving saving)
        {
            _savings.Add(saving);
        }

        public void Unregister(ISaving saving)
        {
            _savings.Remove(saving);
        }

        public ProgressElementsData ProgressElementsData()
        {
            _currentSaveData ??= _saveSystem.Load();

            return _currentSaveData.ProgressElementsData;
        }

        public ProgressCounterData ProgressCounterData()
        {
            _currentSaveData ??= _saveSystem.Load();

            return _currentSaveData.ProgressCounterData;
        }

        private void SaveAll()
        {
            if (_currentSaveData == null)
            {
                _currentSaveData = new SaveData();
            }

            foreach (var saving in _savings)
            {
                saving.SelfSave(_currentSaveData);
                Debug.Log("save self");
            }

            _saveSystem.Save(_currentSaveData);
        }

        private void Awake()
        {
            Debug.Log("Observer created");
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus) SaveAll();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) SaveAll();
        }

        private void OnApplicationQuit()
        {
            SaveAll();
        }

        // public ProgressElementsData ProgressElementsData()
        // {
        //
        // }
    }
}