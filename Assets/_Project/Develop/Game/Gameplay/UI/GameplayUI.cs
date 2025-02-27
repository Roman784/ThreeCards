using Currencies;
using GameplayServices;
using GameState;
using Settings;
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

        private GameSessionStateProvider _gameSessionStateProvider;

        [Inject]
        private void Construct(LevelProgress levelProgress, ChipsCounter chipsCounter, GameplayTools gameplayTools)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
            _gameplayTools = gameplayTools;
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
            _gameplayTools.BindView(_gameplayToolsView);
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

        public void SetToolsServcies(FieldShufflingService fieldShufflingService, 
                                     MagicStickService magicStickService,
                                     LevelRestarterService levelRestarterService)
        {
            _gameplayTools.Init(fieldShufflingService, magicStickService, levelRestarterService);
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
