using GameplayRoot;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using R3;
using LevelMenuRoot;
using BonusWhirlpoolRoot;

namespace GameRoot
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private void RunGame()
        {
            var sceneLoader = new SceneLoader();

#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                var defaultGameplayEnterParams = new GameplayEnterParams(1);
                sceneLoader.LoadAndRunGameplay(defaultGameplayEnterParams);
                return;
            }

            if (sceneName == Scenes.LEVEL_MENU)
            {
                var defaultLevelMenuEnterParams = new LevelMenuEnterParams(1);
                sceneLoader.LoadAndRunLevelMenu(defaultLevelMenuEnterParams);
                return;
            }

            if (sceneName == Scenes.BONUS_WHIRLPOOL)
            {
                var defaultBonusWhirlpoolEnterParams = new BonusWhirlpoolEnterParams(1);
                sceneLoader.LoadAndRunBonusWhirlpool(defaultBonusWhirlpoolEnterParams);
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            var gameplayEnterParams = new GameplayEnterParams(1);
            sceneLoader.LoadAndRunGameplay(gameplayEnterParams);
        }

        
    }
}
