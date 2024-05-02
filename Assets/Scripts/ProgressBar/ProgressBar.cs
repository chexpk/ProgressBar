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

        private void OnElementChanged(int index)
        {
            _barScroll.StartSmoothScrollTo(_progressElementViews[index].GetComponent<RectTransform>());
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
            _counterControl.SetProgressWork(isInprogress);
        }

        private void SetActiveAnimationsCount(bool isPlay)
        {
            var offset = isPlay ? 1 : -1;
            _isAnimationActive += offset;
        }

        // private void GetSnapToPositionToBringChildIntoView(RectTransform child)
        // {
        //     Canvas.ForceUpdateCanvases();
        //     Vector2 viewportLocalPosition = _scrollRect.viewport.localPosition;
        //     Vector2 childLocalPosition   = child.localPosition;
        //     Vector2 result = new Vector2(
        //         0 - (viewportLocalPosition.x + childLocalPosition.x),
        //         0 - (viewportLocalPosition.y + childLocalPosition.y)
        //     );
        //
        //     _scrollRect.content.localPosition =  result;
        // }

        private void OnDestroy()
        {
            foreach (var elementView in _progressElementViews)
            {
                elementView.AnimationPlay -= OnAnimationPlay;
            }

            _progressElementsHandler.ChangedElementIndex -= OnElementChanged;
        }
    }
}