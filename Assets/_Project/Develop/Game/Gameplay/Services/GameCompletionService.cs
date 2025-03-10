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
        private SlotBar _slotBar;
        private Card[,] _cardsMap;

        public GameCompletionService(Observable<List<RemovedCard>> onCardsRemoved, Observable<Card> onCardPlaced,
                                     IGameStateProvider gameStateProvider, ISettingsProvider settingsProvider,
                                     GameplayEnterParams currentGameplayEnterParams,
                                     GameplayUI gameplayUI, SlotBar slotBar, Card[,] cardsMap)
        {
            _gameStateProvider = gameStateProvider;
            _settingsProvider = settingsProvider;
            _currentGameplayEnterParams = currentGameplayEnterParams;
            _gameplayUI = gameplayUI;
            _slotBar = slotBar;
            _cardsMap = cardsMap;

            onCardsRemoved.Subscribe(_ => CheckForWin());
            onCardPlaced.Subscribe(_ => CheckForLose());
        }

        private void CheckForWin()
        {
            if (!_slotBar.HasAnyCard() && !HasCard(_cardsMap))
            {
                _gameStateProvider.GameState.LastPassedLevelNumber.Value = 
                    _currentGameplayEnterParams.LevelNumber;

                var nextLevelNumber = _settingsProvider.GameSettings.CardLayoutsSettings
                    .ClampLevelNumber(_currentGameplayEnterParams.LevelNumber + 1);
                var nextLevelEnterParams = new GameplayEnterParams(nextLevelNumber);

                _gameplayUI.CreateLevelCompletionPopUp(nextLevelEnterParams);
            }
        }

        private void CheckForLose()
        {
            if (!_slotBar.HasEmptySlot())
                _gameplayUI.CreateGameOverPopUp();
        }

        private bool HasCard(Card[,] cardsMap)
        {
            foreach (var card in cardsMap)
            {
                if (card != null && !card.IsDestroyed)
                    return true;
            }

            return false;
        }
    }
}
