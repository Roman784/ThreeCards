using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public CardLayoutsSettings CardLayoutsSettings { get; private set; }
        [field: SerializeField] public SlotsSettings SlotsSettings { get; private set; }
    }
}
