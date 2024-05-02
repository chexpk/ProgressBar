using DefaultNamespace.Progress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.ProgressBar
{
    public class IncreaseProgressView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IProgressCounterIncrease _progressCounter;

        [Inject]
        public void Construct(IProgressCounterIncrease progressCounter)
        {
            _progressCounter = progressCounter;
        }

        public void Awake()
        {
            _button.onClick.AddListener(OnIncrease);
        }

        private void OnIncrease()
        {
            _progressCounter.IncreaseProgress();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnIncrease);
        }
    }
}