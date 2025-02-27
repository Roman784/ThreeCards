using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ToolsSettings", menuName = "Game Settings/Tools/New Tools Settings")]
    public class ToolsSettings : ScriptableObject
    {
        [field: SerializeField] public int FieldShufflingCost { get; private set; }
        [field: SerializeField] public int MagicStickCost { get; private set; }
    }
}
