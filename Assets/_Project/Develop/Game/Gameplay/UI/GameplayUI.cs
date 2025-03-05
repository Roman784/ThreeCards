using Currencies;
using Gameplay;
using GameplayServices;
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
        private SlotBar _slotBar;
        private ISettingsProvider _settingsProvider;

        private SettingsPopUp _settingsPopUp;
        private SettingsPopUp.Factory _settingsPopUpFactory;

        [Inject]
        private void Construct(LevelProgress levelProgress, 
                               ChipsCounter chipsCounter, 
                               GameplayTools gameplayTools, 
                               SlotBar slotBar,
                               ISettingsProvider settingsProvider,
                               SettingsPopUp.Factory settingsPopUpFactory)
        {
            _levelProgress = levelProgress;
            _chipsCounter = chipsCounter;
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
            _settingsProvider = settingsProvider;
            _settingsPopUpFactory = settingsPopUpFactory;

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

        public void CreateBonusSlot()
        {
            var cost = _settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;

            if (_chipsCounter.Count < cost) return;
            _chipsCounter.Reduce(cost);

            _slotBar.CreateBonusSlot();
        }
    }
}
