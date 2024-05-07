using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.System
{
    public class SaveObserver : MonoBehaviour,  ISaver
    {
        private readonly List<ISaving> _savings = new();

        public void RegisterToSave(ISaving saving)
        {
            _savings.Add(saving);
        }

        public void Unregister(ISaving saving)
        {
            _savings.Remove(saving);
        }

        private void SaveAll()
        {
            foreach (var saving in _savings)
            {
                saving.SelfSave();
            }
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
    }
}