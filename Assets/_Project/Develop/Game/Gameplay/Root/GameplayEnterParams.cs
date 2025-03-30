using GameRoot;
using Utils;

namespace GameplayRoot
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public int LevelNumber { get; }
        public float BonusWhirlpoolTimerValue { get; }

        public GameplayEnterParams(int levelNumber, float bonusWhirlpoolTimerValue) : base(Scenes.GAMEPLAY)
        {
            LevelNumber = levelNumber;
            BonusWhirlpoolTimerValue = bonusWhirlpoolTimerValue;
        }
    }
}
