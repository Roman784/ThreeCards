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

            RunGame();
        }

        private static void RunGame()
        {
            _dependenciesInjectedSubj
                .SelectMany(_ => _gameStateProvider.LoadGameState())
                .Subscribe(_ => LoadScene());
        }

        private static void LoadScene()
        {
            var sceneLoader = new SceneLoader();

#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                var defaultGameplayEnterParams = new GameplayEnterParams(GetCurrentLevelNumber());
                sceneLoader.LoadAndRunGameplay(defaultGameplayEnterParams);
                return;
            }

            if (sceneName == Scenes.LEVEL_MENU)
            {
                var defaultLevelMenuEnterParams = new LevelMenuEnterParams(GetCurrentLevelNumber());
                sceneLoader.LoadAndRunLevelMenu(defaultLevelMenuEnterParams);
                return;
            }

            if (sceneName == Scenes.BONUS_WHIRLPOOL)
            {
                var defaultBonusWhirlpoolEnterParams = new BonusWhirlpoolEnterParams(GetCurrentLevelNumber());
                sceneLoader.LoadAndRunBonusWhirlpool(defaultBonusWhirlpoolEnterParams);
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            var gameplayEnterParams = new GameplayEnterParams(GetCurrentLevelNumber());
            sceneLoader.LoadAndRunGameplay(gameplayEnterParams);
        }

        private static int GetCurrentLevelNumber()
        {
            var levelNumber = _gameStateProvider.GameState.LastPassedLevelNumber.Value + 1;
            return _settingsProvider.GameSettings.CardLayoutsSettings.ClampLevelNumber(levelNumber);
        }
    }
}
