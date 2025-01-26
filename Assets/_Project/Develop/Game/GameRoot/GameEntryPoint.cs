using GameplayRoot;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using R3;

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

        private GameEntryPoint()
        {

        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                Coroutines.StartRoutine(LoadAndStartGameplay());
                return;
            }

            if (sceneName == Scenes.LEVEL_MENU)
            {
                Coroutines.StartRoutine(LoadAndStartLevelMenu());
                return;
            }

            if (sceneName == Scenes.BONUS_WHIRLPOOL)
            {
                Coroutines.StartRoutine(LoadAndStartBonusWhirlpool());
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            Coroutines.StartRoutine(LoadAndStartGameplay());
        }

        private IEnumerator LoadAndStartGameplay()
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.GAMEPLAY);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            sceneEntryPoint.Run();
        }

        private IEnumerator LoadAndStartLevelMenu()
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.LEVEL_MENU);

            var sceneEntryPoint = Object.FindObjectOfType<LevelMenuEntryPoint>();
            sceneEntryPoint.Run();
        }

        private IEnumerator LoadAndStartBonusWhirlpool()
        {
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.BONUS_WHIRLPOOL);

            var sceneEntryPoint = Object.FindObjectOfType<BonusWhirlpoolEntryPoint>();
            sceneEntryPoint.Run();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
