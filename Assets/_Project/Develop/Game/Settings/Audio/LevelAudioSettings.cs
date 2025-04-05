using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "LevelAudioSettings", menuName = "Game Settings/Audio/New Level Audio Settings")]
    public class LevelAudioSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip CardsMatchedSound { get; private set; }
        [field: SerializeField] public AudioClip LevelCompletedSound { get; private set; }
        [field: SerializeField] public AudioClip GameOverSound { get; private set; }
        [field: SerializeField] public AudioClip TimerSound { get; private set; }
    }
}
