using UnityEngine;

namespace System
{
    [CreateAssetMenu(fileName = "DefaultDataSettings", menuName = "Progress/DefaultDataSettings")]
    public class DefaultDataSettings : ScriptableObject
    {

        [field: SerializeField] public int CountOfProgressElements { get; private set; } = 8;
        [field: SerializeField] public float DelayToReceived { get; set; } = 40f;
    }
}