using UnityEngine;

namespace LevelMenu
{
    public class LevelButton
    {
        private LevelButtonView _view;

        private int _number;

        public LevelButton(LevelButtonView view)
        {
            _view = view;
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void SetNumber(int number)
        {
            _number = number;
            _view?.SetNumber(_number);
        }
    }
}
