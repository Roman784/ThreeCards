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

namespace GameRoot
{
    public class GameEntryPoint : MonoBehaviour
    {
        private static IGameStateProvider _gameStateProvider;
        private static ISettingsProvider _settingsProvider;

        private static Subject<Unit> _dependenciesInjectedSubj = new();

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider, ISettingsProvider settingsProvider)
        {
            _gameStateProvider = gameStateProvider;
            _settingsProvider = settingsProvider;

            _dependenciesInjectedSubj.OnNext(Unit.Default);
            _dependenciesInjectedSubj.OnCompleted();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            var activeSceneName = SceneManager.GetActiveScene().name;
            RunGame(activeSceneName);

            new SceneLoader().LoadBoot();
        }

        private static void RunGame(string activeSceneName = Scenes.MAIN_MENU)
        {
            _dependenciesInjectedSubj
                .SelectMany(_ => _gameStateProvider.LoadGameState())
                .Subscribe(_ => LoadScene(activeSceneName));
        }

        private static void LoadScene(string activeSceneName = Scenes.MAIN_MENU)
        {
            var sceneLoader = new SceneLoader();

#if UNITY_EDITOR
            if (activeSceneName == Scenes.MAIN_MENU)
            {
                var defaultMainMenuEnterParams = new MainMenuEnterParams(GetCurrentLevelNumber());
                sceneLoader.LoadAndRunMainMenu(defaultMainMenuEnterParams);
                return;
            }

            if (activeSceneName == Scenes.GAMEPLAY)
            {
                var defaultGameplayEnterParams = new GameplayEnterParams(GetCurrentLevelNumber(), 0);
                sceneLoader.LoadAndRunGameplay(defaultGameplayEnterParams);
                return;
            }

            if (activeSceneName == Scenes.LEVEL_MENU)
            {
                var defaultLevelMenuEnterParams = new LevelMenuEnterParams(GetCurrentLevelNumber(), 0);
                sceneLoader.LoadAndRunLevelMenu(defaultLevelMenuEnterParams);
                return;
            }

            if (activeSceneName == Scenes.BONUS_WHIRLPOOL)
            {
                var defaultBonusWhirlpoolEnterParams = new BonusWhirlpoolEnterParams(GetCurrentLevelNumber());
                sceneLoader.LoadAndRunBonusWhirlpool(defaultBonusWhirlpoolEnterParams);
                return;
            }

            if (activeSceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            var mainMenuEnterParams = new MainMenuEnterParams(GetCurrentLevelNumber());
            sceneLoader.LoadAndRunMainMenu(mainMenuEnterParams);
        }

        private static int GetCurrentLevelNumber()
        {
            var levelNumber = _gameStateProvider.GameState.LastPassedLevelNumber.Value + 1;
            return _settingsProvider.GameSettings.CardLayoutsSettings.ClampLevelNumber(levelNumber);
        }
    }
}
