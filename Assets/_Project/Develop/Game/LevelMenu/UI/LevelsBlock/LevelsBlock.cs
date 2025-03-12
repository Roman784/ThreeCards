using UnityEngine;

namespace LevelMenu
{
    public class LevelsBlock
    {
        private LevelsBlockView _view;

        private bool _isOpen;

        public LevelsBlock(LevelsBlockView view)
        {
            _view = view;
            _isOpen = false;

            _view.OnOpenClose += () => OpenClose();
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void SetLevelNumberRange(Vector2Int levelNumberRange)
        {
            _view?.SetLevelNumberRange(levelNumberRange);
        }

        public void CreateLevelButtons(Vector2Int levelNumberRange)
        {
            _view?.CreateLevelButtons(levelNumberRange);
        }

        private void OpenClose()
        {
            if (_isOpen)
                _view.Close();
            else
                _view.Open();

            _isOpen = !_isOpen;
        }
    }
}
