using Cysharp.Threading.Tasks;
using DefaultNamespace.Progress;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class AppLoader : MonoBehaviour
    {
        [field: SerializeField] private string SecondSceneName { get; set; } = "MainMenu";

        private IProgressElementsCreator _progressElementsCreator;

        [Inject]
        public void Construct(IProgressElementsCreator progressElementsCreator)
        {
            _progressElementsCreator = progressElementsCreator;
        }

        private async void Start()
        {
            var elements = _progressElementsCreator.ProgressElements;
            await SceneManager.LoadSceneAsync(SecondSceneName);
        }
    }
}