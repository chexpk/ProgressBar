using UnityEngine;

namespace DefaultNamespace.Progress.Settings
{
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "Progress/ProgressSettings")]
    public class ProgressSettings : ScriptableObject
    {
        [field: SerializeField] public float IncreaseProgressButtonValue = 100f;
        [field: SerializeField] public float ProgressElementValue = 756f;
        [field: SerializeField] public float IncreaseSpeed = 80f;
    }
}