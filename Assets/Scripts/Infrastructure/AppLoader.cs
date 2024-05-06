using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class AppLoader : MonoBehaviour
    {
        [field: SerializeField] private string SecondSceneName { get; set; } = "MainMenu";

        private async void Start()
        {
            await SceneManager.LoadSceneAsync(SecondSceneName);
        }
    }
}