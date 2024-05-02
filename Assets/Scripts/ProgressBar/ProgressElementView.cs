using System;
using System.Threading.Tasks;
using DefaultNamespace.Progress;
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
        }

        private async void OnAchieved()
        {
            OnProgressChanged(1f);
            var task = StartAnimation();
            AnimationPlay?.Invoke(true);
            await task;
            AnimationPlay?.Invoke(false);
            // if (!task.Result)
            // {
            //     return;
            // }
        }

        private void OnProgressChanged(float progress)
        {
            _slider.value = progress;
        }

        private void Awake()
        {
            _slider.value = 0;
            transform.gameObject.transform.SetParent(_parent);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _slider.value = _progressElement.SelfProgress;
        }

        private Task<bool> StartAnimation()
        {
            return _animationHandler.Play();
        }

        public class Factory : PlaceholderFactory<Transform, ProgressElement, ProgressElementView>{ }
    }
}