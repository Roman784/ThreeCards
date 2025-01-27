using GameRoot;
using Utils;

namespace GameplayRoot
{
    public class GameplayEnterParams : SceneEnterParams
    {
        public int LevelNumber { get; }

        public GameplayEnterParams(int levelNumber) : base(Scenes.GAMEPLAY)
        {
            LevelNumber = levelNumber;
        }
    }
}
