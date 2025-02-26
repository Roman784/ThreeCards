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

        private LevelProgress _levelProgress;
        private ChipsCounter _chipsCounter;

        private FieldShufflingService _fieldShufflingService;

        [Inject]
        private void Construct(LevelProgress levelProgress, ChipsCounter chipsCounter)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                _fieldShufflingService?.Shuffle();
        }

        public void BindViews()
        {
            _levelProgress.BindView(_levelProgressView);
            _chipsCounter.BindView(_chipsCounterView);
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
            _fieldShufflingService = fieldShufflingService;
        }
    }
}
