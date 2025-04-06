using GameRoot;
using Utils;

namespace GameplayRoot
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public int LevelNumber { get; }
        public float BonusWhirlpoolTimerValue { get; }

        public GameplayEnterParams(string exitSceneName, int levelNumber, float bonusWhirlpoolTimerValue) : base(Scenes.GAMEPLAY, exitSceneName)
        {
            LevelNumber = levelNumber;
            BonusWhirlpoolTimerValue = bonusWhirlpoolTimerValue;
        }
    }
}
