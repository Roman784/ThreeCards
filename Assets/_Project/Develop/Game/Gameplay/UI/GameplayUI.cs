using Currencies;
using GameplayServices;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private LevelProgressView _levelProgressView;
        [SerializeField] private ChipsCounterView _chipsCounterView;
        [SerializeField] private GameplayToolsView _gameplayToolsView;

        private LevelProgress _levelProgress;
        private ChipsCounter _chipsCounter;
        private GameplayTools _gameplayTools;

        [Inject]
        private void Construct(LevelProgress levelProgress, ChipsCounter chipsCounter)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;

            _gameplayTools = new();
        }

        public void BindViews()
        {
            _levelProgress.BindView(_levelProgressView);
            _chipsCounter.BindView(_chipsCounterView);
            _gameplayTools.BindView(_gameplayToolsView);
        }

        public void InitChips(CardMatchingService cardMatchingService)
        {
            _chipsCounter.InitChips(cardMatchingService);
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelProgress.SetLevelNumber(levelNumber);
        }

        public void InitProgressBar(int totalCardCount, CardMatchingService cardMatchingService)
        {
            _levelProgress.InitProgressBar(totalCardCount, cardMatchingService);
        }

        public void SetToolsServcies(FieldShufflingService fieldShufflingService)
        {
            _gameplayTools.Init(fieldShufflingService);
        }

        public void EnableTools()
        {
            _gameplayTools.Enable();
        }

        public void DisableTools()
        {
            _gameplayTools.Disable();
        }
    }
}
