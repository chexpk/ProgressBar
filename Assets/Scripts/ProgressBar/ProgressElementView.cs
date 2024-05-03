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
        public event Action<bool> AnimationPlay;

        [SerializeField] private Slider _slider;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private ElementAnimationHandler _animationHandler;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GameObject _gameObject;

        private Transform _parent;
        //TODO I_ProgressElement
        private ProgressElement _progressElement;
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
        }

        private void Start()
        {
            _slider.value = _progressElement.SelfProgress;
        }

        private async void OnAchieved()
        {
            OnProgressChanged(1f);
            var task = StartAnimation();
            AnimationPlay?.Invoke(true);
            await task;
            if (!task.Result)
            {
                return;
            }
            AnimationPlay?.Invoke(false);

            _timerText.gameObject.SetActive(true);
        }

        private void OnTimeChanged(float leftTime)
        {
            int minutes = Mathf.FloorToInt(leftTime / 60f);
            int seconds = Mathf.FloorToInt(leftTime - minutes * 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void OnReceived()
        {
            _timerText.gameObject.SetActive(false);
            _gameObject.gameObject.SetActive(true);
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