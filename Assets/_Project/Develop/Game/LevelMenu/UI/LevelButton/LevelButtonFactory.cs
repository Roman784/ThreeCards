using Zenject;

namespace LevelMenu
{
    public class LevelButtonFactory : PlaceholderFactory<LevelButtonView>
    {
        public new LevelButton Create()
        {
            var view = base.Create();
            return new LevelButton(view);
        }

        public LevelButton Create(int number)
        {
            var button = Create();
            button.SetNumber(number);

            return button;
        }
    }
}
