using Currencies;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private LevelProgressView _levelProgressView;
        [SerializeField] private ChipsCounterView _chipsCounterView;

        private LevelProgress _levelProgress;
        private ChipsCounter _chipsCounter;

        [Inject]
        private void Construct(LevelProgress levelProgress, ChipsCounter chipsCounter)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
        }

        public void BindViews()
        {
            _levelProgress.BindView(_levelProgressView);
            _chipsCounter.BindView(_chipsCounterView);
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelProgress.SetLevelNumber(levelNumber);
        }
    }
}
