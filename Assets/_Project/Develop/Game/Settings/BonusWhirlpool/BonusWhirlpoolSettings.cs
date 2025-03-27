using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "BonusWhirlpoolSettings", menuName = "Game Settings/Bonus Whirlpool/Bonus Whirlpool Settings")]
    public class BonusWhirlpoolSettings : ScriptableObject
    {
        [field: SerializeField] public float TimerValue { get; private set; }
        [field: SerializeField] public float TimerValueOffset { get; private set; }

        [field: Space]

        [field: SerializeField] public BonusWhirlpoolSlotsSettings SlotsSettings {  get; private set; }
        [field: SerializeField] public CardWhirlpoolSettings CardSettings { get; private set; }
    }
}
