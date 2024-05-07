using System;

namespace DefaultNamespace.System
{
    [Serializable]
    public record CounterData : Data
    {
        public float Progress;
        public float AskedProgress;
    }
}