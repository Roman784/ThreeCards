using Gameplay;
using GameplayServices;
using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;
using R3;
using Utils;
using CameraUtils;
using System.Collections;
using GameRoot;
using LevelMenuRoot;
using Audio;

namespace GameplayRoot
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private GameplayUI _gameplayUI;
        private GameplayPopUpProvider _popUpProvider;
        private ISettingsProvider _settingsProvider;
        private SlotBar _slotBar;
        private CardFactory _cardFactory;
        private ShakyCamera _shakyCamera;
        private AudioPlayer _audioPlayer;

        private Coroutine _cardLayOutSoundRoutine;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               GameplayUI gameplayUI,
                               GameplayPopUpProvider popUpProvider,
                               ISettingsProvider settingsProvider,
                               SlotBar slotBar,
                               CardFactory cardFactory,
                               ShakyCamera shakyCamera,
                               AudioPlayer audioPlayer)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _gameplayUI = gameplayUI;
            _popUpProvider = popUpProvider;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
            _shakyCamera = shakyCamera;
            _audioPlayer = audioPlayer;
        }

        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<GameplayEnterParams>());
        }

        private IEnumerator Run(GameplayEnterParams enterParams)
        {
            if (!IsLevelExist(enterParams.LevelNumber))
                LoadLevelMenu(enterParams);

            var isLoaded = false;

            _gameStateProvider.LoadGameState().Subscribe(_ =>
            {
                // Settings data.
                var gameSettings = _settingsProvider.GameSettings;
                var layouts = gameSettings.CardLayoutsSettings;
                var layout = layouts.GetLayout(enterParams.LevelNumber);
                var audioSettings = gameSettings.AudioSettings;

                // Fiel setup.
                var slots = _slotBar.CreateSlots();

                var cardPlacingService = new CardPlacingService(_slotBar);
                var onCardPlaced = cardPlacingService.OnCardPlaced;
                var onCardReadyToPlaced = cardPlacingService.OnCardReadyToPlaced;

                var cardMatchingService = new CardMatchingService(_slotBar, onCardPlaced);
                var onCardsRemoved = cardMatchingService.OnCardsRemoved;

                onCardsRemoved.Subscribe(_ => cardPlacingService.ShiftCards());

                var cardLayoutService = new CardLayoutService(layouts, _cardFactory, cardPlacingService);
                var cardMarkingService = new CardMarkingService();

                var cardsMap = cardLayoutService.SetUp(layout);
                var totalCardCount = CollectionsCounter.CountOfNonNullItems(cardsMap);

                var fieldService = new FieldService(cardsMap, _slotBar);

                cardMarkingService.Mark(fieldService, layout.CardSpreadRange);

                // Animations.
                var cardFlippingService = new CardFlippingService(fieldService, onCardReadyToPlaced, onCardsRemoved);
                var fieldAnimationService = new FieldAnimationService(fieldService, cardFlippingService);
                var onLayOutAnimationCompleted = fieldAnimationService.LayOutCards();

                // Audio.
                var cardPutDownSound = audioSettings.CardAudioSettings.PutDownSound;
                _cardLayOutSoundRoutine = _audioPlayer.PlayAnyTimes(cardPutDownSound, totalCardCount, 0.1f, onLayOutAnimationCompleted);

                onCardPlaced.Subscribe(_ => _audioPlayer.PlayOneShot(audioSettings.SlotAudioSettings.CardPlacementSound));

                // UI.
                var fieldShufflingService = new FieldShufflingService(fieldService, cardFlippingService);
                var magicStickService = new MagicStickService(fieldService, cardMatchingService, cardLayoutService);
                var levelRestarterService = new LevelRestarterService(fieldService, _shakyCamera, 
                                                                      enterParams.LevelNumber, _gameplayUI.BonusWhirlpoolTransition,
                                                                      _audioPlayer, audioSettings);

                _uiRoot.AttachSceneUI(_gameplayUI.gameObject);
                _gameplayUI.BindViews();

                _gameplayUI.SetGameplayEnterParams(enterParams);
                _gameplayUI.InitProgressBar(totalCardCount, onCardsRemoved);
                _gameplayUI.InitBonusMenu();
                _gameplayUI.InitChips(onCardsRemoved);

                _gameplayUI.SetToolsServcies(fieldShufflingService, magicStickService, levelRestarterService);
                onLayOutAnimationCompleted.Subscribe(_ => _gameplayUI.EnableTools());

                // Winning and losing.
                var gameCompletionService = new GameCompletionService(onCardsRemoved, onCardPlaced,
                                                                        _gameStateProvider, _settingsProvider,
                                                                        enterParams, _popUpProvider, fieldService,
                                                                        _gameplayUI.BonusWhirlpoolTransition);

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }

        private void OnDestroy()
        {
            if (_cardLayOutSoundRoutine != null)
                Coroutines.StopRoutine(_cardLayOutSoundRoutine);
        }

        private bool IsLevelExist(int levelNumber)
        {
            return _settingsProvider.GameSettings.CardLayoutsSettings.IsLevelExist(levelNumber);
        }

        private void LoadLevelMenu(GameplayEnterParams enterParams)
        {
            var levelMenuEnterParams = new LevelMenuEnterParams(enterParams.LevelNumber, 
                                                                _gameplayUI.BonusWhirlpoolTransition.CurrentTimerValue);
            new SceneLoader().LoadAndRunLevelMenu(levelMenuEnterParams);
        }
    }
}
