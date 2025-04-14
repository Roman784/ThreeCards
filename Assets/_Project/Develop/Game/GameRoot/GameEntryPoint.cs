using GameplayRoot;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using LevelMenuRoot;
using BonusWhirlpoolRoot;
using Zenject;
using GameState;
using R3;
using System;
using Settings;
using MainMenuRoot;
using Localization;
using SDK;

namespace GameRoot
{
    public class GameEntryPoint : MonoBehaviour
    {
        private static IGameStateProvider _gameStateProvider;
        private static ISettingsProvider _settingsProvider;
        private static ILocalizationProvider _localizationProvider;
        private static SDK.SDK _sdk;

        private static Subject<Unit> _dependenciesInjectedSubj = new();

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider, ISettingsProvider settingsProvider,
                               ILocalizationProvider localizationProvider, SDK.SDK sdk)
        {
            _gameStateProvider = gameStateProvider;
            _settingsProvider = settingsProvider;
            _localizationProvider = localizationProvider;
            _sdk = sdk;

            _dependenciesInjectedSubj.OnNext(Unit.Default);
            _dependenciesInjectedSubj.OnCompleted();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            RunGame();
            new SceneLoader().LoadBoot();
        }

        private static void RunGame()
        {
            _dependenciesInjectedSubj
                .Subscribe(_ => _sdk.Init().Subscribe(res =>
                {
                    if (res) _gameStateProvider.LoadGameState().Subscribe(res =>
                    {
                        var language = _sdk.GetLanguage();
                        Debug.Log(language);
                        if (res != null) _localizationProvider.LoadTranslations(language).Subscribe(_ =>
                        {
                            LoadMainMenu();
                        });
                    });
                }
            ));
        }

        private static void LoadMainMenu()
        {
            _sdk.GameReady();

            var mainMenuEnterParams = new MainMenuEnterParams(Scenes.BOOT);
            new SceneLoader().LoadAndRunMainMenu(mainMenuEnterParams);
        }
    }
}
