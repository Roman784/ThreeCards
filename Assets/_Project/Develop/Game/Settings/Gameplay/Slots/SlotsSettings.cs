using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "SlotsSettings", menuName = "Game Settings/Slots/New Slots Settings")]
    public class SlotsSettings : ScriptableObject
    {
        [field: SerializeField] public int Count {  get; private set; }
    }
}
