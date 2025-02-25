using Currencies;
using GameplayServices;
using GameState;
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

        private GameSessionStateProvider _gameSessionStateProvider;

        [Inject]
        private void Construct(LevelProgress levelProgress, ChipsCounter chipsCounter)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                _gameSessionStateProvider.SetLastState();
            }
        }

        public void BindViews()
        {
            _levelProgress.BindView(_levelProgressView);
            _chipsCounter.BindView(_chipsCounterView);
        }

        public void SetGameSessionStateProvider(GameSessionStateProvider gameSessionStateProvider)
        {
            _gameSessionStateProvider = gameSessionStateProvider;
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
    }
}
