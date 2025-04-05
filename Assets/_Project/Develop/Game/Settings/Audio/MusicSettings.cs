using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "MusicSettings", menuName = "Game Settings/Audio/New Music Settings")]
    public class MusicSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip MainTheme { get; private set; }
        [field: SerializeField] public AudioClip BonusLevel { get; private set; }
    }
}
