using UnityEngine;

namespace DefaultNamespace.Progress.Settings
{
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "Progress/ProgressSettings")]
    public class ProgressSettings : ScriptableObject
    {
        [field: SerializeField] public float IncreaseProgressButtonValue { get; private set; } = 30f;
        [field: SerializeField] public float ProgressElementValue { get; private set; } = 100f;
        [field: SerializeField] public float IncreaseSpeed { get; private set; } = 0.8f;
    }
}