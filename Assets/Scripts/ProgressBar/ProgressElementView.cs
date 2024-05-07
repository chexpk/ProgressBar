using System;
using System.Threading.Tasks;
using DefaultNamespace.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.ProgressBar
{
    public class ProgressElementView : MonoBehaviour
    {
        public event Action PlayStarted;
        public event Action PlayStopped;

        [SerializeField] private Slider _slider;
        [SerializeField] private ElementAnimationHandler _animationHandler;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GameObject _receivedIcon;

        private ProgressElement _progressElement;
        private Transform _parent;
        private bool _isInProcess = false;

        [Inject]
        public void Construct(Transform parent, ProgressElement progressElement)
        {
            _parent = parent;
            _progressElement = progressElement;

            _progressElement.SelfProgressChanged += OnProgressChanged;
            _progressElement.Achieved += OnAchieved;
            _progressElement.TimeReceivedChanged += OnTimeChanged;
            _progressElement.Received += OnReceived;
        }

        private void Awake()
        {
            _slider.value = 0;
            transform.gameObject.transform.SetParent(_parent);
            if (_progressElement.IsAchieved && !_progressElement.IsReceived)
            {
                _timerText.gameObject.SetActive(true);
            }

            if (_progressElement.IsReceived)
            {
                _receivedIcon.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            _slider.value = _progressElement.SelfProgress;
        }

        private async void OnAchieved(ProgressElement element)
        {
            OnProgressChanged(1f);
            var task = StartAnimation();
            PlayStarted?.Invoke();
            await task;
            if (!task.Result)
            {
                return;
            }
            PlayStopped?.Invoke();

            if (_progressElement.IsReceived)
            {
                return;
            }

            _timerText.gameObject.SetActive(true);
        }

        private void OnTimeChanged(float leftTime)
        {
            int minutes = Mathf.FloorToInt(leftTime / 60f);
            int seconds = Mathf.FloorToInt(leftTime - minutes * 60);
            _timerText.text = $"{minutes:0}:{seconds:00}";
        }

        private void OnReceived(ProgressElement element)
        {
            _timerText.gameObject.SetActive(false);
            _receivedIcon.gameObject.SetActive(true);
        }

        private void OnProgressChanged(float progress)
        {
            _slider.value = progress;
        }

        private Task<bool> StartAnimation()
        {
            return _animationHandler.Play();
        }

        private void OnDestroy()
        {
            _progressElement.SelfProgressChanged -= OnProgressChanged;
            _progressElement.Achieved -= OnAchieved;
            _progressElement.TimeReceivedChanged -= OnTimeChanged;
            _progressElement.Received -= OnReceived;
        }

        public class Factory : PlaceholderFactory<Transform, ProgressElement, ProgressElementView>{ }
    }
}