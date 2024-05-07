using System;
using DefaultNamespace.Progress;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Reward
{
    public class RewardView : MonoBehaviour
    {
        public event Action<RewardView> Received;
        [SerializeField] private TMP_Text _timerText;

        private Transform _parent;
        private ProgressElement _element;

        [Inject]
        public void Construct(Transform parent, ProgressElement progressElement)
        {
            _parent = parent;
            _element = progressElement;

            _element.Received += OnReceived;
            _element.TimeReceivedChanged += OnTimeChanged;
        }

        private void OnReceived(ProgressElement element)
        {
            Received?.Invoke(this);
        }

        private void Awake()
        {
            transform.gameObject.transform.SetParent(_parent);
            transform.SetAsLastSibling();
        }

        private void Start()
        {
            SetTimeToText(_element.TimeToReceived);
        }

        private void OnTimeChanged(float leftTime)
        {
            SetTimeToText(leftTime);
        }

        private void SetTimeToText(float leftTime)
        {
            int minutes = Mathf.FloorToInt(leftTime / 60f);
            int seconds = Mathf.FloorToInt(leftTime - minutes * 60);
            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void OnDestroy()
        {
            _element.Received -= OnReceived;
            _element.TimeReceivedChanged -= OnTimeChanged;
        }


        public class Factory : PlaceholderFactory<Transform, ProgressElement, RewardView> { }
    }
}