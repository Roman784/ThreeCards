using GameRoot;
using Utils;

namespace LevelMenuRoot
{
    public class LevelMenuEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }

        public LevelMenuEnterParams(int currentLevelNumber) : base(Scenes.LEVEL_MENU)
        {
            CurrentLevelNumber = currentLevelNumber;
        }
    }
}
