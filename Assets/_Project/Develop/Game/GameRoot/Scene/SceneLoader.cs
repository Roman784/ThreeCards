using BonusWhirlpoolRoot;
using GameplayRoot;
using LevelMenuRoot;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GameRoot
{
    public class SceneLoader
    {
        private UIRootView UI => Object.FindObjectOfType<UIRootView>();

        public void LoadBoot()
        {
            Coroutines.StartRoutine(LoadSceneRoutine(Scenes.BOOT));
        }

        public void LoadAndRunGameplay(GameplayEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunSceneRoutine<GameplayEntryPoint, GameplayEnterParams>(Scenes.GAMEPLAY, enterParams));
        }

        public void LoadAndRunLevelMenu(LevelMenuEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunSceneRoutine<LevelMenuEntryPoint, LevelMenuEnterParams>(Scenes.LEVEL_MENU, enterParams));
        }

        public void LoadAndRunBonusWhirlpool(BonusWhirlpoolEnterParams enterParams)
        {
            Coroutines.StartRoutine(LoadAndRunSceneRoutine<BonusWhirlpoolEntryPoint, BonusWhirlpoolEnterParams>(Scenes.BONUS_WHIRLPOOL, enterParams));
        }

        private IEnumerator LoadAndRunSceneRoutine<TEntryPoint, TEnterParams>(string sceneName, TEnterParams enterParams) 
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            yield return UI?.ShowLoadingScreen();

            yield return LoadSceneRoutine(sceneName);

            var sceneEntryPoint = Object.FindObjectOfType<TEntryPoint>();
            yield return sceneEntryPoint.Run(enterParams);

            yield return UI?.HideLoadingScreen();
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}
