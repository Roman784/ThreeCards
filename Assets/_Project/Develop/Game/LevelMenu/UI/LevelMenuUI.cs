using GameplayRoot;
using GameRoot;
using GameState;
using Settings;
using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelMenuUI : MonoBehaviour
    {
        [SerializeField] private Transform _levelBlcoksContainer;
        [SerializeField] private int _levelsInBlock;

        private ISettingsProvider _settingsProvider;
        private IGameStateProvider _gameStateProvider;
        private LevelsBlockFactory _levelsBlockFactory;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, IGameStateProvider gameStateProvider, 
                               LevelsBlockFactory levelsBlockFactory)
        {
            _settingsProvider = settingsProvider;
            _gameStateProvider = gameStateProvider;
            _levelsBlockFactory = levelsBlockFactory;
        }

        public void OpenLevel(int number)
        {
            var gameplayEnterParams = new GameplayEnterParams(number);
            new SceneLoader().LoadAndRunGameplay(gameplayEnterParams);
        }

        public void CreateLevelsBlocks()
        {
            int levelsCount = _settingsProvider.GameSettings.CardLayoutsSettings.LayoutsCount;
            int blocksCount = Mathf.CeilToInt(levelsCount / (float)_levelsInBlock);

            for (int i = 0; i < blocksCount; i++)
            {
                int startLevelNumber = i * _levelsInBlock + 1;
                int endLevelNumber = (i == blocksCount - 1)
                    ? levelsCount
                    : startLevelNumber + _levelsInBlock - 1;

                Vector2Int levelNumberRange = new Vector2Int(startLevelNumber, endLevelNumber);

                CreateLevelsBlock(levelNumberRange);
            }
        }

        private void CreateLevelsBlock(Vector2Int levelNumberRange)
        {
            var levelsBlock = _levelsBlockFactory.Create(levelNumberRange);
            levelsBlock.Attach(_levelBlcoksContainer);
            levelsBlock.CreateLevelButtons(levelNumberRange, this);
        }
    }
}
