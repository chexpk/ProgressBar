using System;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    [Serializable]
    public class ProgressElement : ISerializationCallbackReceiver
    {
        public event Action<float> SelfProgressChanged;
        public event Action<ProgressElement> Achieved;
        public event Action<float> TimeReceivedChanged;
        public event Action Received;

        public DateTime TimeOfAchieved { get; private set; }
        [field: SerializeField] public bool IsAchieved { get; private set; }
        [field: SerializeField] public bool IsReceived { get; private set; }
        [field: SerializeField] public float SelfProgress { get; private set; }
        [field: SerializeField] public int IconIndex { get; private set; }
        [field: SerializeField] public float TimeToReceived { get; private set; }
        [field: SerializeField] public string _timeOfAchieved { get; private set; }
        [field: SerializeField] public float DelayToReceived { get; private set; }

        public ProgressElement(bool isAchieved, bool isReceived, DateTime timeOfAchieved, float selfProgress, int iconIndex,
            float delayToReceived)
        {
            IsAchieved = isAchieved;
            IsReceived = isReceived;
            TimeOfAchieved = timeOfAchieved;
            SelfProgress = selfProgress;
            IconIndex = iconIndex;
            DelayToReceived = delayToReceived;
        }

        public void SetSelfProgress(float progress)
        {
            SelfProgress = progress;
            SelfProgressChanged?.Invoke(SelfProgress);
        }

        public void SetMaxProgress(DateTime achievedUtcTime)
        {
            TimeOfAchieved = achievedUtcTime;
            SelfProgress = 1f;
            IsAchieved = true;
            Achieved?.Invoke(this);
            StartReceiveTimer();
        }

        private void SetTimeToReceived(float secondToReceived)
        {
            TimeToReceived = secondToReceived;
            TimeReceivedChanged?.Invoke(TimeToReceived);
        }

        public async void StartReceiveTimer()
        {
            var timer = new ElementsReceiveTimer(TimeOfAchieved, DelayToReceived, SetTimeToReceived);
            var task = timer.Launch();
            await task;
            if (!task.Result)
            {
                return;
            }

            IsReceived = true;
            Received?.Invoke();
        }

        public void OnBeforeSerialize()
        {
            _timeOfAchieved = TimeOfAchieved.ToString("o");
        }

        public void OnAfterDeserialize()
        {
            DateTime.TryParse(_timeOfAchieved, out DateTime time);
            TimeOfAchieved = time.ToUniversalTime();
        }
    }
}