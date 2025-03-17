using Gameplay;
using GameplayRoot;
using GameState;
using R3;
using Settings;
using System.Collections.Generic;
using UI;
using UnityEngine;
using static GameplayServices.CardMatchingService;

namespace GameplayServices
{
    public class GameCompletionService
    {
        private IGameStateProvider _gameStateProvider;
        private ISettingsProvider _settingsProvider;
        private GameplayEnterParams _currentGameplayEnterParams;
        private GameplayUI _gameplayUI;
        private FieldService _fieldService;

        public GameCompletionService(Observable<List<RemovedCard>> onCardsRemoved, Observable<Card> onCardPlaced,
                                     IGameStateProvider gameStateProvider, ISettingsProvider settingsProvider,
                                     GameplayEnterParams currentGameplayEnterParams,
                                     GameplayUI gameplayUI, FieldService fieldService)
        {
            _gameStateProvider = gameStateProvider;
            _settingsProvider = settingsProvider;
            _currentGameplayEnterParams = currentGameplayEnterParams;
            _gameplayUI = gameplayUI;
            _fieldService = fieldService;

            onCardsRemoved.Subscribe(_ => CheckForWin());
            onCardPlaced.Subscribe(_ => CheckForLose());
        }

        private void CheckForWin()
        {
            if (!_fieldService.SlotBar.HasAnyCard() && !_fieldService.HasAnyCard())
            {
                var currentLevelNumber = _currentGameplayEnterParams.LevelNumber;
                var layoutsSettings = _settingsProvider.GameSettings.CardLayoutsSettings;
                var lastPassedLevelNumber = _gameStateProvider.GameState.LastPassedLevelNumber;

                if (currentLevelNumber > lastPassedLevelNumber.Value)
                    lastPassedLevelNumber.Value = currentLevelNumber;

                var nextLevelNumber = layoutsSettings.ClampLevelNumber(currentLevelNumber + 1);
                var nextLevelEnterParams = new GameplayEnterParams(nextLevelNumber);

                _gameplayUI.CreateLevelCompletionPopUp(nextLevelEnterParams);
            }
        }

        private void CheckForLose()
        {
            if (!_fieldService.SlotBar.HasEmptySlot())
                _gameplayUI.CreateGameOverPopUp();
        }
    }
}
