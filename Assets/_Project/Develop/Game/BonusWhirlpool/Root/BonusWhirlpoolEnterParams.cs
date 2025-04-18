using GameRoot;
using Utils;

namespace BonusWhirlpoolRoot
{
    public class BonusWhirlpoolEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }

        public BonusWhirlpoolEnterParams(string exitSceneName, int currentLevelNumber) : base(Scenes.BONUS_WHIRLPOOL, exitSceneName)
        {
            CurrentLevelNumber = currentLevelNumber;
        }
    }
}
