using Currencies;
using Gameplay;
using GameplayRoot;
using GameplayServices;
using GameRoot;
using LevelMenuRoot;
using R3;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private LevelProgressView _levelProgressView;
        [SerializeField] private ChipsCounterView _chipsCounterView;
        [SerializeField] private GameplayToolsView _gameplayToolsView;

        private GameplayEnterParams _gameplayEnterParams;

        private LevelProgress _levelProgress;
        private ChipsCounter _chipsCounter;
        private GameplayTools _gameplayTools;
        private SlotBar _slotBar;
        private GameplayPopUpProvider _popUpProvider;

        public GameplayTools GameplayTools => _gameplayTools;

        [Inject]
        private void Construct(LevelProgress levelProgress,
                               ChipsCounter chipsCounter,
                               GameplayTools gameplayTools,
                               SlotBar slotBar,
                               GameplayPopUpProvider popUpProvider)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
            _popUpProvider = popUpProvider;

            _slotBar.BonusSlotView.OnCreate += () => CreateBonusSlot();
        }

        public void BindViews()
        {
            _levelProgress.BindView(_levelProgressView);
            _chipsCounter.BindView(_chipsCounterView);
            _gameplayTools.BindView(_gameplayToolsView);
        }

        public void OpenSettings()
        {
            _popUpProvider.OpenSettingsPopUp();
        }

        public void OpenLevelMenu()
        {
            var currentLevelNumber = _gameplayEnterParams.LevelNumber;
            var levelMenuEnterParams = new LevelMenuEnterParams(currentLevelNumber);
            new SceneLoader().LoadAndRunLevelMenu(levelMenuEnterParams);
        }

        public void InitChips(Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved)
        {
            _chipsCounter.InitChips(onCardsRemoved);
        }

        public void SetGameplayEnterParams(GameplayEnterParams gameplayEnterParams)
        {
            _gameplayEnterParams = gameplayEnterParams;
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelProgress.SetLevelNumber(levelNumber);
        }

        public void InitProgressBar(int totalCardCount, Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved)
        {
            _levelProgress.InitProgressBar(totalCardCount, onCardsRemoved);
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

        public void CreateBonusSlot()
        {
            _popUpProvider.OpenBonuSlotPopUp();
        }
    }
}
