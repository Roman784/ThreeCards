using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public InitialGameStateSettings InitialGameStateSettings { get; private set; }

        [field: Space]

        [field: SerializeField] public CardLayoutsSettings CardLayoutsSettings { get; private set; }
        [field: SerializeField] public SlotsSettings SlotsSettings { get; private set; }
        [field: SerializeField] public ToolsSettings ToolsSettings { get; private set; }
        [field: SerializeField] public BonusWhirlpoolSettings BonusWhirlpoolSettings { get; private set; }

        [field: Space]

        [field: SerializeField] public AudioSettings AudioSettings { get; private set; }
    }
}
