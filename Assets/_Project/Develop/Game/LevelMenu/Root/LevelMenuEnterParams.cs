using GameRoot;
using Utils;

namespace LevelMenuRoot
{
    public class LevelMenuEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }
        public float BonusWhirlpoolTimerValue { get; }

        public LevelMenuEnterParams(string exitSceneName, int currentLevelNumber, float bonusWhirlpoolTimerValue) : base(Scenes.LEVEL_MENU, exitSceneName)
        {
            CurrentLevelNumber = currentLevelNumber;
            BonusWhirlpoolTimerValue = bonusWhirlpoolTimerValue;
        }
    }
}
