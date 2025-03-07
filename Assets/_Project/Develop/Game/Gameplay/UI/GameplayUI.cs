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
        private ISettingsProvider _settingsProvider;

        private SettingsPopUp _settingsPopUp;
        private SettingsPopUp.Factory _settingsPopUpFactory;

        private AdvertisingChipsPopUp _advertisingChipsPopUp;
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactroy;

        [Inject]
        private void Construct(LevelProgress levelProgress, 
                               ChipsCounter chipsCounter, 
                               GameplayTools gameplayTools, 
                               SlotBar slotBar,
                               ISettingsProvider settingsProvider,
                               SettingsPopUp.Factory settingsPopUpFactory,
                               AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactroy)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
            _settingsProvider = settingsProvider;
            _settingsPopUpFactory = settingsPopUpFactory;
            _advertisingChipsPopUpFactroy = advertisingChipsPopUpFactroy;

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
            var cost = _settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;

            if (!CheckCost(cost)) return;
            _chipsCounter.Reduce(cost);

            _slotBar.CreateBonusSlot();
        }

        private bool CheckCost(int cost)
        {
            if (_chipsCounter.Count >= cost) return true;

            OpenAdvertisingChipsPopUp();
            return false;
        }

        private void OpenAdvertisingChipsPopUp()
        {
            if (_advertisingChipsPopUp == null)
                _advertisingChipsPopUp = _advertisingChipsPopUpFactroy.Create();

            _advertisingChipsPopUp.Open();
        }
    }
}
