using System;
using System.Collections.Generic;
using DefaultNamespace.Progress;
using UnityEngine.Serialization;

namespace DefaultNamespace.System
{
    [Serializable]
    public class SaveData
    {
        public ProgressElementsData ProgressElementsData;
        public ProgressCounterData ProgressCounterData;
    }

    [Serializable]
    public record ProgressElementsData
    {
        public List<ProgressElement> ProgressElements;
    }

    [Serializable]
    public record ProgressCounterData
    {
        public float Progress;
        public float AskedProgress;
    }
}