using System.Collections.Generic;
using DefaultNamespace.Progress;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Reward
{
    public class RewardBar : MonoBehaviour
    {
        [SerializeField] private Transform _rootElementViews;

        private IProgressElementsHandler _progressElementsHandler;
        private RewardView.Factory _rewardViewFactory;
        private readonly List<RewardView> _progressElementViews = new();
        private readonly List<ProgressElement> _notAchievedElements = new();

        [Inject]
        public void Construct(IProgressElementsHandler progressElementsHandler, RewardView.Factory rewardViewFactory)
        {
            _progressElementsHandler = progressElementsHandler;
            _rewardViewFactory = rewardViewFactory;
        }

        private void Start()
        {
            _progressElementViews.Clear();

            foreach (var element in _progressElementsHandler.ProgressElements)
            {
                if (element.IsReceived)
                {
                    continue;
                }

                if (!element.IsAchieved)
                {
                    _notAchievedElements.Add(element);
                    element.Achieved += OnAchieved;
                    continue;
                }

                var rewardView = _rewardViewFactory.Create(_rootElementViews, element);
                rewardView.gameObject.transform.SetParent(_rootElementViews);
                _progressElementViews.Add(rewardView);
                rewardView.Received += OnReceived;
            }
        }

        private void OnReceived(RewardView rewardView)
        {
            rewardView.Received += OnReceived;
            _progressElementViews.Remove(rewardView);
            Destroy(rewardView.gameObject);
        }

        private void OnAchieved(ProgressElement element)
        {
            var rewardView = _rewardViewFactory.Create(_rootElementViews, element);
            rewardView.gameObject.transform.SetParent(_rootElementViews);
            _progressElementViews.Add(rewardView);
            rewardView.Received += OnReceived;
        }

        private void OnDestroy()
        {
            foreach (var element in _notAchievedElements)
            {
                element.Achieved -= OnAchieved;
            }

            foreach (var rewardView in _progressElementViews)
            {
                rewardView.Received -= OnReceived;
            }
        }
    }
}