using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "UIAudioSettings", menuName = "Game Settings/Audio/New UI Audio Settings")]
    public class UIAudioSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip ButtonClickSound { get; private set; }
        [field: SerializeField] public AudioClip LevelBlockOpenCloseSound { get; private set; }
    }
}
