using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Game Settings/Audio/New Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [field: SerializeField] public CardAudioSettings CardAudioSettings {  get; private set; }
        [field: SerializeField] public SlotAudioSettings SlotAudioSettings { get; private set; }
    }
}
