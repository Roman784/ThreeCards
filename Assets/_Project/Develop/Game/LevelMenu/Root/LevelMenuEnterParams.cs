using GameRoot;
using Utils;

namespace LevelMenuRoot
{
    public class LevelMenuEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }
        public float BonusWhirlpoolTimerValue { get; }

        public LevelMenuEnterParams(int currentLevelNumber, float bonusWhirlpoolTimerValue) : base(Scenes.LEVEL_MENU)
        {
            CurrentLevelNumber = currentLevelNumber;
            BonusWhirlpoolTimerValue = bonusWhirlpoolTimerValue;
        }
    }
}
