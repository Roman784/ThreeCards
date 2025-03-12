using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelsBlockFactory : PlaceholderFactory<LevelsBlockView>
    {
        public new LevelsBlock Create()
        {
            var view = base.Create();
            return new LevelsBlock(view);
        }

        public LevelsBlock Create(Vector2Int levelNumberRange)
        {
            var block = Create();
            block.SetLevelNumberRange(levelNumberRange);

            return block;
        }
    }
}
