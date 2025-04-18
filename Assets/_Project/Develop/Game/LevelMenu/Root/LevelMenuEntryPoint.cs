using GameRoot;
using GameState;
using System.Collections;
using UnityEngine;
using Zenject;
using R3;
using UI;
using LevelMenu;
using Settings;

namespace LevelMenuRoot
{
    public class LevelMenuEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private SDK.SDK _sdk;
        private UIRootView _uiRoot;
        private LevelMenuUI _levelMenuUI;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               SDK.SDK sdk,
                               UIRootView uiRoot,
                               LevelMenuUI levelMenuUI)
        {
            _gameStateProvider = gameStateProvider;
            _sdk = sdk;
            _uiRoot = uiRoot;
            _levelMenuUI = levelMenuUI;
        }

        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<LevelMenuEnterParams>());
        }

        private IEnumerator Run(LevelMenuEnterParams enterParams)
        {
            var isLoaded = false;

            _gameStateProvider.LoadGameState().Subscribe(_ =>
            {
                _uiRoot.AttachSceneUI(_levelMenuUI.gameObject);

                _levelMenuUI.SetLevelMenuEnterParams(enterParams);
                _levelMenuUI.BindViews();
                _levelMenuUI.InitChips();
                _levelMenuUI.CreateLevelsBlocks();
                _levelMenuUI.LocalizeTexts();

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
