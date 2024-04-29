using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class AppLoader : MonoBehaviour
    {
        [field: SerializeField] private string SecondSceneName { get; set; } = "MainMenu";

        private void Start()
        {
            SceneManager.LoadSceneAsync(SecondSceneName);
        }
    }
}