using BonusWhirlpoolRoot;
using GameplayRoot;
using LevelMenuRoot;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GameRoot
{
    public class SceneLoader
    {
        public void LoadAndRunGameplay(GameplayEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunGameplayRoutine(enterParams));
        }

        public void LoadAndRunLevelMenu(LevelMenuEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunLevelMenuRoutine(enterParams));
        }

        public void LoadAndRunBonusWhirlpool(BonusWhirlpoolEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunBonusWhirlpoolRoutine(enterParams));
        }

        private IEnumerator LoadAndRunGameplayRoutine(GameplayEnterParams enterParams)
        {
            yield return LoadSceneRoutine(Scenes.BOOT);
            yield return LoadSceneRoutine(Scenes.GAMEPLAY);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadAndRunLevelMenuRoutine(LevelMenuEnterParams enterParams)
        {
            yield return LoadSceneRoutine(Scenes.BOOT);
            yield return LoadSceneRoutine(Scenes.LEVEL_MENU);

            var sceneEntryPoint = Object.FindObjectOfType<LevelMenuEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadAndRunBonusWhirlpoolRoutine(BonusWhirlpoolEnterParams enterParams)
        {
            yield return LoadSceneRoutine(Scenes.BOOT);
            yield return LoadSceneRoutine(Scenes.BONUS_WHIRLPOOL);

            var sceneEntryPoint = Object.FindObjectOfType<BonusWhirlpoolEntryPoint>();
            sceneEntryPoint.Run(enterParams);
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}
