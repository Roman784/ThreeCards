using Zenject;

namespace LevelMenu
{
    public class LevelButtonFactory : PlaceholderFactory<LevelButtonView>
    {
        public new LevelButton Create()
        {
            var view = base.Create();
            return new LevelButton(view, 0, false, null);
        }

        public LevelButton Create(int number, bool isPassed, LevelMenuUI levelMenu)
        {
            var view = base.Create();
            return new LevelButton(view, number, isPassed, levelMenu);
        }
    }
}
