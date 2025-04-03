using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "InitialGameStateSettings", menuName = "Game Settings/New Initial Game State Settings")]
    public class InitialGameStateSettings : ScriptableObject
    {
        [field: SerializeField] public GameState.GameState GameState { get; private set; }
    }
}
