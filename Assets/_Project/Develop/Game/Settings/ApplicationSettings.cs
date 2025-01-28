using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Game Settings/New Application Settings")]
    public class ApplicationSettings : ScriptableObject
    {
        [field: SerializeField] public float SoundVolume { get; private set; }
        [field: SerializeField] public string Language { get; private set; }
    }
}
