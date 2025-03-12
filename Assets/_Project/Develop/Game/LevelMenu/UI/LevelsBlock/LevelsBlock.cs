using UnityEngine;

namespace LevelMenu
{
    public class LevelsBlock
    {
        private LevelsBlockView _view;

        public LevelsBlock(LevelsBlockView view)
        {
            _view = view;
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
    }
}
