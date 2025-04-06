using Currencies;
using GameplayRoot;
using GameRoot;
using GameState;
using LevelMenuRoot;
using Settings;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace LevelMenu
{
    public class LevelMenuUI : SceneUI
    {
        [Space]

        [SerializeField] private Transform _levelBlcoksContainer;
        [SerializeField] private int _levelsInBlock;

        [Space]

        [SerializeField] private Transform _commingSoonView;

        private LevelMenuEnterParams _enterParams;
        private LevelsBlockFactory _levelsBlockFactory;

        [Inject]
        private void Construct(LevelsBlockFactory levelsBlockFactory)
        {
            _levelsBlockFactory = levelsBlockFactory;
        }

        public void SetLevelMenuEnterParams(LevelMenuEnterParams enterParams)
        {
            _enterParams = enterParams;
        }

        public void OpenLastAvailableLevel()
        {
            PlayButtonClickSound();

            var number = _gameStateProvider.GameState.LastPassedLevelNumber.Value + 1;
            OpenLevel(Scenes.LEVEL_MENU, number, _enterParams.BonusWhirlpoolTimerValue);
        }

        public void BackToLastScene()
        {
            PlayButtonClickSound();

            if (_enterParams.ExitSceneName == Scenes.GAMEPLAY)
                OpenLevel(_enterParams.CurrentLevelNumber);
            else
                OpenMainMenu(Scenes.LEVEL_MENU);
        }

        public void OpenLevel(int number)
        {
            base.OpenLevel(Scenes.LEVEL_MENU, number, _enterParams.BonusWhirlpoolTimerValue);
        }

        public void CreateLevelsBlocks()
        {
            int levelsCount = _settingsProvider.GameSettings.CardLayoutsSettings.LayoutsCount;
            int blocksCount = Mathf.CeilToInt(levelsCount / (float)_levelsInBlock);
            int lastPassedLevelNumber = _gameStateProvider.GameState.LastPassedLevelNumber.Value;

            for (int i = 0; i < blocksCount; i++)
            {
                int startLevelNumber = i * _levelsInBlock + 1;
                int endLevelNumber = (i == blocksCount - 1)
                    ? levelsCount
                    : startLevelNumber + _levelsInBlock - 1;

                Vector2Int levelNumberRange = new Vector2Int(startLevelNumber, endLevelNumber);
                int completedLevelsInBlock = Mathf.Max(0, Mathf.Min(lastPassedLevelNumber, endLevelNumber) - startLevelNumber + 1);
                int currentBlockLevelCount = endLevelNumber - startLevelNumber + 1;
                float progress = Mathf.Clamp01((float)completedLevelsInBlock / currentBlockLevelCount);

                CreateLevelsBlock(levelNumberRange, progress, lastPassedLevelNumber);
            }

            _commingSoonView.SetAsLastSibling();
        }

        private void CreateLevelsBlock(Vector2Int levelNumberRange, float progress, int lastPassedLevelNumber)
        {
            var levelsBlock = _levelsBlockFactory.Create(levelNumberRange, progress);
            levelsBlock.Attach(_levelBlcoksContainer);
            levelsBlock.CreateLevelButtons(levelNumberRange, lastPassedLevelNumber, this);
        }
    }
}
