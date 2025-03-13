using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelMenu
{
    public class LevelsBlock
    {
        private LevelsBlockView _view;
        private float _progress;

        private bool _isOpen;

        public LevelsBlock(LevelsBlockView view, Vector2Int levelNumberRange, float progress)
        {
            _view = view;
            _progress = progress;
            _isOpen = false;

            _view.SetLevelNumberRange(levelNumberRange);
            _view.SetProgress(_progress);

            _view.OnOpenClose += () => OpenClose();
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void CreateLevelButtons(Vector2Int levelnumberRange, float progress, LevelMenuUI levelMenu)
        {
            for (int number = levelnumberRange.x; number <= levelnumberRange.y; number++)
            {
                var isPassed = Mathf.Clamp01(1f - (float)(levelnumberRange.y - number + 1) / (levelnumberRange.y - levelnumberRange.x + 1)) < progress;
                Debug.Log($"{number} | {(float)(levelnumberRange.y - number + 1) / (levelnumberRange.y - levelnumberRange.x + 1)} | {progress}");
                CreateLevelButton(number, isPassed, levelMenu);
            }
        }

        private void OpenClose()
        {
            if (_isOpen)
                _view.Close();
            else
                _view.Open();

            _isOpen = !_isOpen;
        }

        private void CreateLevelButton(int number, bool isPassed, LevelMenuUI levelMenu)
        {
            var button = _view?.CreateLevelButton(number, isPassed, levelMenu);
            _view?.AttachButton(button);
        }
    }
}
