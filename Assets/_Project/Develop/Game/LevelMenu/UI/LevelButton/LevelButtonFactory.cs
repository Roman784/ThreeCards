using Zenject;

namespace LevelMenu
{
    public class LevelButtonFactory : PlaceholderFactory<LevelButtonView>
    {
        public new LevelButton Create()
        {
            var view = base.Create();
            return new LevelButton(view, 0, null);
        }

        public LevelButton Create(int number, LevelMenuUI levelMenu)
        {
            var view = base.Create();
            return new LevelButton(view, number, levelMenu);
        }
    }
}
