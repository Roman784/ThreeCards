using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "SlotAudioSettings", menuName = "Game Settings/Audio/New Slot Audio Settings")]
    public class SlotAudioSettings : ScriptableObject
    {
        [field: SerializeField] public AudioClip CardPlacementSound { get; private set; }
    }
}
