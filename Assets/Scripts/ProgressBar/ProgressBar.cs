using System.Collections.Generic;
using DefaultNamespace.Progress;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.ProgressBar
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private BarScroll _barScroll;
        [SerializeField] private Transform _rootElementViews;
        private IProgressElementsHandler _progressElementsHandler;
        private ProgressElementView.Factory _progressElementFactory;
        private readonly List<ProgressElementView> _progressElementViews = new();
        private IProgressCounterControl _counterControl;

        private int _isAnimationActive = 0;

        [Inject]
        public void  Construct(IProgressElementsHandler progressElementsHandler,
            ProgressElementView.Factory progressElementFactory,
            IProgressCounterControl counterControl)
        {
            _progressElementsHandler = progressElementsHandler;
            _progressElementFactory = progressElementFactory;
            _counterControl = counterControl;

            _progressElementsHandler.ChangedElementIndex += OnElementChanged;
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
                elementView.PlayStarted += OnPlayStarted;
                elementView.PlayStopped += OnPlayStopped;
            }
        }

        private void OnElementChanged(int index)
        {
            _barScroll.StartSmoothScrollTo(_progressElementViews[index].GetComponent<RectTransform>());
        }

        private void OnPlayStopped()
        {
            ControlProgressCounterPause(false);
        }

        private void OnPlayStarted()
        {
            ControlProgressCounterPause(true);
        }

        private void ControlProgressCounterPause(bool isPlay)
        {
            var offset = isPlay ? 1 : -1;
            _isAnimationActive += offset;

            var isInprogress = _isAnimationActive == 0;
            _counterControl.SetProgressWork(isInprogress);
        }

        private void OnDestroy()
        {
            foreach (var elementView in _progressElementViews)
            {
                elementView.PlayStarted -= OnPlayStarted;
                elementView.PlayStopped -= OnPlayStopped;
            }

            _progressElementsHandler.ChangedElementIndex -= OnElementChanged;

        }
    }
}