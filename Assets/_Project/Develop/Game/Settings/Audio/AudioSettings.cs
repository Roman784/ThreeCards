using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Game Settings/Audio/New Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [field: SerializeField] public UIAudioSettings UIAudioSettings { get; private set; }
        [field: SerializeField] public CardAudioSettings CardAudioSettings {  get; private set; }
        [field: SerializeField] public SlotAudioSettings SlotAudioSettings { get; private set; }
        [field: SerializeField] public ChipsAudioSettings ChipsAudioSettings { get; private set; }
        [field: SerializeField] public LevelAudioSettings LevelAudioSettings { get; private set; }

        [field: Space]

        [field: SerializeField] public MusicSettings MusicSettings { get; private set; }
    }
}
