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
    }
}
