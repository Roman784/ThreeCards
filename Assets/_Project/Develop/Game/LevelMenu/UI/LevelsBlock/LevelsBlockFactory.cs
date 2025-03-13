using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelsBlockFactory : PlaceholderFactory<LevelsBlockView>
    {
        public new LevelsBlock Create()
        {
            var view = base.Create();
            return new LevelsBlock(view, Vector2Int.zero, 0f);
        }

        public LevelsBlock Create(Vector2Int levelNumberRange, float progress)
        {
            var view = base.Create();
            return new LevelsBlock(view, levelNumberRange, progress);
        }
    }
}
