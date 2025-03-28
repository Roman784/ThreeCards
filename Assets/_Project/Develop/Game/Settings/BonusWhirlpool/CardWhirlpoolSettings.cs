using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CardWhirlpoolSettings", menuName = "Game Settings/Bonus Whirlpool/New Card Whirlpool Settings")]
    public class CardWhirlpoolSettings : ScriptableObject
    {
        [field: SerializeField] public int Count {  get; private set; }
        [field: SerializeField] public Vector2 Radius { get; private set; }
        [field: SerializeField] public Vector2 RadiusOffset { get; private set; }
        [field: SerializeField] public float FlightSpeed { get; private set; }
        [field: SerializeField] public float FlightSpeedOffset { get; private set; }
        [field: SerializeField] public float TrajectoryAngleOffset { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeedOffset { get; private set; }
        [field: SerializeField] public Vector2 PositionOffset { get; private set; }
    }
}
