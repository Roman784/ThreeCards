using GameRoot;
using GameState;
using System.Collections;
using UnityEngine;
using Zenject;
using R3;
using UI;
using LevelMenu;

namespace LevelMenuRoot
{
    public class LevelMenuEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private LevelMenuUI _levelMenuUI;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               LevelMenuUI levelMenuUI)
        {
            _gameStateProvider = gameStateProvider;
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

                _levelMenuUI.BindViews();
                _levelMenuUI.InitChips();
                _levelMenuUI.SetCurrentLevelNumber(enterParams.CurrentLevelNumber);
                _levelMenuUI.CreateLevelsBlocks();

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
