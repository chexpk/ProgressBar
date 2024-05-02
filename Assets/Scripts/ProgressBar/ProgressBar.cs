using System.Collections.Generic;
using DefaultNamespace.Progress;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.ProgressBar
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform _rootElementViews;
        private IProgressElementsHandler _progressElementsHandler;
        private ProgressElementView.Factory _progressElementFactory;
        private readonly List<ProgressElementView> _progressElementViews = new();
        private IProgressCounterPause _counterPause;

        private int _isAnimationActive = 0;

        [Inject]
        public void  Construct(IProgressElementsHandler progressElementsHandler,
            ProgressElementView.Factory progressElementFactory,
            IProgressCounterPause counterPause)
        {
            _progressElementsHandler = progressElementsHandler;
            _progressElementFactory = progressElementFactory;
            _counterPause = counterPause;
        }

        private void Start()
        {
            InitViews();
        }

        private void InitViews()
        {
            _progressElementViews.Clear();

            foreach (var element in _progressElementsHandler.ProgressElements)
            {
                var elementView = _progressElementFactory.Create(_rootElementViews, element);
                elementView.gameObject.transform.SetParent(_rootElementViews);
                _progressElementViews.Add(elementView);
                elementView.AnimationPlay += OnAnimationPlay;
            }
        }

        private void OnAnimationPlay(bool isPlay)
        {
            SetActiveAnimationsCount(isPlay);
            ControlProgressCounterPause();
        }

        private void ControlProgressCounterPause()
        {
            var isInprogress = _isAnimationActive == 0;
            _counterPause.IsProgressWork = isInprogress;
        }

        private void SetActiveAnimationsCount(bool isPlay)
        {
            var offset = isPlay ? 1 : -1;
            _isAnimationActive += offset;
        }

        private void OnDestroy()
        {
            foreach (var elementView in _progressElementViews)
            {
                elementView.AnimationPlay -= OnAnimationPlay;
            }
        }
    }
}