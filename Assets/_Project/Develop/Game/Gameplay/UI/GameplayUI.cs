using Currencies;
using Gameplay;
using GameplayServices;
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

        private LevelProgress _levelProgress;
        private ChipsCounter _chipsCounter;
        private GameplayTools _gameplayTools;
        private SlotBar _slotBar;

        private SettingsPopUp _settingsPopUp;
        private SettingsPopUp.Factory _settingsPopUpFactory;

        private BonusSlotPopUp _bonusSlotPopUp;
        private BonusSlotPopUp.Factory _bonusSlotPopUpfactory;

        [Inject]
        private void Construct(LevelProgress levelProgress,
                               ChipsCounter chipsCounter,
                               GameplayTools gameplayTools,
                               SlotBar slotBar,
                               SettingsPopUp.Factory settingsPopUpFactory,
                               BonusSlotPopUp.Factory bonusSlotPopUpFactory)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
            _settingsPopUpFactory = settingsPopUpFactory;
            _bonusSlotPopUpfactory = bonusSlotPopUpFactory;

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
            if (_settingsPopUp == null)
                _settingsPopUp = _settingsPopUpFactory.Create();

            _settingsPopUp.Open();
        }

        public void InitChips(Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved)
        {
            _chipsCounter.InitChips(onCardsRemoved);
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
            if (_bonusSlotPopUp == null)
                _bonusSlotPopUp = _bonusSlotPopUpfactory.Create();

            _bonusSlotPopUp.Open();
        }
    }
}
