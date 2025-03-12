using TMPro;
using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelsBlockView : MonoBehaviour
    {
        [SerializeField] private Transform _levelsContainer;
        [SerializeField] private TMP_Text _levelNumberRangeView;

        private LevelButtonFactory _levelButtonFactory;

        [Inject]
        private void Construct(LevelButtonFactory levelButtonFactory)
        {
            _levelButtonFactory = levelButtonFactory;
        }

        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetLevelNumberRange(Vector2Int levelNumberRange)
        {
            _levelNumberRangeView.text = $"{levelNumberRange.x} - {levelNumberRange.y}";
        }

        public void CreateLevelButtons(Vector2Int levelnumberRange)
        {
            for (int number = levelnumberRange.x; number <= levelnumberRange.y; number++)
            {
                CreateLevelButton(number);
            }
        }

        private void CreateLevelButton(int number)
        {
            var button = _levelButtonFactory.Create(number);
            button.Attach(_levelsContainer);
        }
    }
}
