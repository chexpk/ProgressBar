using System;

namespace DefaultNamespace.Progress
{
    public class ProgressElement
    {
        public event Action<float> SelfProgressChanged;
        public event Action Achieved;
        public event Action Received;

        public bool IsAchieved { get; private set; }
        public bool IsReceived { get; private set; }
        public float TimeOfAchieved { get; private set; }
        public float SelfProgress { get; private set; }
        public int IconIndex { get; private set; }


        public ProgressElement(bool isAchieved, bool isReceived, float timeOfAchieved, float selfProgress, int iconIndex)
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

        public void SetMaxProgress()
        {
            SelfProgress = 1f;
            Achieved?.Invoke();
        }
    }
}