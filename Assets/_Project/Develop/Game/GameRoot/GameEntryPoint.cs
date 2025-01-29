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
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                var defaultGameplayEnterParams = new GameplayEnterParams(1);
                Coroutines.StartRoutine(LoadAndStartGameplay(defaultGameplayEnterParams));
                return;
            }

            if (sceneName == Scenes.LEVEL_MENU)
            {
                var defaultLevelMenuEnterParams = new LevelMenuEnterParams(1);
                Coroutines.StartRoutine(LoadAndStartLevelMenu(defaultLevelMenuEnterParams));
                return;
            }

            if (sceneName == Scenes.BONUS_WHIRLPOOL)
            {
                var defaultBonusWhirlpoolEnterParams = new BonusWhirlpoolEnterParams(1);
                Coroutines.StartRoutine(LoadAndStartBonusWhirlpool(defaultBonusWhirlpoolEnterParams));
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            var gameplayEnterParams = new GameplayEnterParams(1);
            Coroutines.StartRoutine(LoadAndStartGameplay(gameplayEnterParams));
        }

        private IEnumerator LoadAndStartGameplay(GameplayEnterParams enterParams)
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.GAMEPLAY);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadAndStartLevelMenu(LevelMenuEnterParams enterParams)
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.LEVEL_MENU);

            var sceneEntryPoint = Object.FindObjectOfType<LevelMenuEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadAndStartBonusWhirlpool(BonusWhirlpoolEnterParams enterParams)
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.BONUS_WHIRLPOOL);

            var sceneEntryPoint = Object.FindObjectOfType<BonusWhirlpoolEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}
