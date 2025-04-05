using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ChipsAudioSettings", menuName = "Game Settings/Audio/New Chips Audio Settings")]
    public class ChipsAudioSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip CollectionSound { get; private set; }
        [field: SerializeField] public AudioClip ReduceSound { get; private set; }
    }
}
