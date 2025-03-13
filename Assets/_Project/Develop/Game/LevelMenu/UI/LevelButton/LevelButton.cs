using System;
using UnityEngine;

namespace LevelMenu
{
    public class LevelButton
    {
        private LevelButtonView _view;

        private int _number;
        private LevelMenuUI _levelMenu;

        public LevelButton(LevelButtonView view, int number, bool isPassed, LevelMenuUI levelMenu)
        {
            _view = view;
            _number = number;
            _levelMenu = levelMenu;

            _view.SetNumber(_number);
            _view.Fill(isPassed);

            _view.OnOpenLevel += () => OpenLevel();
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void OpenLevel()
        {
            _levelMenu?.OpenLevel(_number);
        }
    }
}
