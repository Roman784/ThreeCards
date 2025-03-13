using Zenject;

namespace LevelMenu
{
    public class LevelButtonFactory : PlaceholderFactory<LevelButtonView>
    {
        public new LevelButton Create()
        {
            var view = base.Create();
            return new LevelButton(view, 0, true, false, null);
        }

        public LevelButton Create(int number, bool isPassed, bool isLocked, LevelMenuUI levelMenu)
        {
            var view = base.Create();
            return new LevelButton(view, number, isPassed, isLocked, levelMenu);
        }
    }
}
