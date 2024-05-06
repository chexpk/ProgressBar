using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Progress;
using DefaultNamespace.System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class AppLoader : MonoBehaviour
    {
        [field: SerializeField] private string SecondSceneName { get; set; } = "MainMenu";

        private IDefaultDataCreator _defaultDataCreator;
        private ISaveSystem _saveSystem;

        [Inject]
        public void Construct(IDefaultDataCreator defaultDataCreator, ISaveSystem saveSystem)
        {
            _defaultDataCreator = defaultDataCreator;
            _saveSystem = saveSystem;
        }

        private async void Start()
        {
            // var elements = _progressElementsCreator.ProgressElements;
            // var saveData = new SaveData()
            // {
            //     ProgressElements = _progressElementsCreator.ProgressElements,
            //     ProgressCounterData = new ProgressCounterData()
            //     {
            //         AskedProgress = 0, Progress = 0,
            //     }
            // };
            //
            // _saveSystem.Save(saveData);


            await SceneManager.LoadSceneAsync(SecondSceneName);
        }
    }
}