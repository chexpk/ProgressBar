using System;

namespace DefaultNamespace.Progress
{
    public class ProgressElement
    {
        public event Action<float> SelfProgressChanged;
        public event Action<ProgressElement> Achieved;
        public event Action<float> TimeReceivedChanged;
        public event Action Received;

        public bool IsAchieved { get; private set; }
        public bool IsReceived { get; private set; }
        public DateTime TimeOfAchieved { get; private set; }
        public float SelfProgress { get; private set; }
        public int IconIndex { get; private set; }
        public float TimeToReceived { get; private set; } = 0;


        public ProgressElement(bool isAchieved, bool isReceived, DateTime timeOfAchieved, float selfProgress, int iconIndex)
        {
            IsAchieved = isAchieved;
            IsReceived = isReceived;
            TimeOfAchieved = timeOfAchieved;
            SelfProgress = selfProgress;
            IconIndex = iconIndex;
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

        public void SetTimeToReceived(float secondToReceived)
        {
            TimeToReceived = secondToReceived;
            TimeReceivedChanged?.Invoke(TimeToReceived);
        }

        private async void StartReceiveTimer()
        {
            var timer = new ElementsReceiveTimer(this);
            var task = timer.Launch();
            await task;
            if (!task.Result)
            {
                return;
            }

            IsReceived = true;
            Received?.Invoke();
        }
    }
}