using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BonusWhirlpoolSlotsSettings", menuName = "Game Settings/Slots/New Bonus Whirlpool Slots Settings")]
    public class BonusWhirlpoolSlotsSettings : ScriptableObject
    {
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public int CountInRow { get; private set; }
    }
}