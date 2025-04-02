using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CardAudioSettings", menuName = "Game Settings/Audio/New Card Audio Settings")]
    public class CardAudioSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip PutDownSound { get; private set; }
        [field: SerializeField] public AudioClip RotationSound { get; private set; }
        [field: SerializeField] public AudioClip MovementSound { get; private set; }
    }
}
