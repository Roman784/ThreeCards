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
using Utils;
using Zenject;

namespace UI
{
    public class GameplayUI : SceneUI
    {
        [SerializeField] private LevelProgressView _levelProgressView;
        [SerializeField] private GameplayToolsView _gameplayToolsView;
        [SerializeField] private BonusWhirlpoolTransitionView _bonusWhirlpoolTransitionView;

        private GameplayEnterParams _gameplayEnterParams;

        private LevelProgress _levelProgress;
        private GameplayTools _gameplayTools;
        private BonusWhirlpoolTransition _bonusWhirlpoolTransition;
        private SlotBar _slotBar;
        private GameplayPopUpProvider _gameplayPopUpProvider;
        private SDK.SDK _sdk;

        public GameplayTools GameplayTools => _gameplayTools;
        public BonusWhirlpoolTransition BonusWhirlpoolTransition => _bonusWhirlpoolTransition;

        [Inject]
        private void Construct(LevelProgress levelProgress,
                               GameplayTools gameplayTools,
                               BonusWhirlpoolTransition bonusWhirlpoolTransition,
                               SlotBar slotBar,
                               GameplayPopUpProvider gameplayPopUpProvider,
                               SDK.SDK sdk)
        {
            _levelProgress = levelProgress;
            _gameplayTools = gameplayTools;
            _bonusWhirlpoolTransition = bonusWhirlpoolTransition;
            _slotBar = slotBar;
            _gameplayPopUpProvider = gameplayPopUpProvider;
            _sdk = sdk;

            _slotBar.BonusSlotView.OnCreate += () => CreateBonusSlot();
        }

        private void OnDestroy()
        {
            _bonusWhirlpoolTransition?.StopTimer();
        }

        public override void BindViews()
        {
            base.BindViews();

            _levelProgress.BindView(_levelProgressView);
            _gameplayTools.BindView(_gameplayToolsView);
            _bonusWhirlpoolTransition.BindView(_bonusWhirlpoolTransitionView);
        }

        public void OpenLevelMenu()
        {
            _sdk.ShowFullscreenAdv();

            PlayButtonClickSound();

            var levelMenuEnterParams = new LevelMenuEnterParams(Scenes.GAMEPLAY,
                                                                _gameplayEnterParams.LevelNumber,
                                                                _bonusWhirlpoolTransition.CurrentTimerValue);
            new SceneLoader().LoadAndRunLevelMenu(levelMenuEnterParams);
        }

        public void SetGameplayEnterParams(GameplayEnterParams gameplayEnterParams)
        {
            _gameplayEnterParams = gameplayEnterParams;
        }

        public void InitProgressBar(int totalCardCount, Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved)
        {
            _levelProgress.InitProgressBar(totalCardCount, onCardsRemoved);
            _levelProgress.SetLevelNumber(_gameplayEnterParams.LevelNumber);
        }

        public void InitBonusMenu()
        {
            var bonusWhirlpoolTimerValue = _settingsProvider.GameSettings.BonusWhirlpoolSettings.Cooldown;

            _bonusWhirlpoolTransition.Init(_gameplayPopUpProvider, _gameplayEnterParams, _audioPlayer, AudioSettings);
            _bonusWhirlpoolTransition.StartTimer(bonusWhirlpoolTimerValue, _gameplayEnterParams.BonusWhirlpoolTimerValue);
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
            PlayButtonClickSound();
            _gameplayPopUpProvider.OpenBonuSlotPopUp();
        }
    }
}
