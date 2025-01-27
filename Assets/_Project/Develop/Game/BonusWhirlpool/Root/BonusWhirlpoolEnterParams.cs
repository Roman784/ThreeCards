using GameRoot;
using Utils;

namespace BonusWhirlpoolRoot
{
    public class BonusWhirlpoolEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }

        public BonusWhirlpoolEnterParams(int currentLevelNumber) : base(Scenes.BONUS_WHIRLPOOL)
        {
            CurrentLevelNumber = currentLevelNumber;
        }
    }
}
