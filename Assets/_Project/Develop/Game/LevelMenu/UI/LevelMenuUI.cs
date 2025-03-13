using Currencies;
using GameplayRoot;
using GameRoot;
using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelMenuUI : MonoBehaviour
    {
        [SerializeField] private ChipsCounterView _chipsCounterView;

        [Space]

        [SerializeField] private Transform _levelBlcoksContainer;
        [SerializeField] private int _levelsInBlock;

        private ISettingsProvider _settingsProvider;
        private IGameStateProvider _gameStateProvider;
        private LevelsBlockFactory _levelsBlockFactory;
        private ChipsCounter _chipsCounter;

        private SettingsPopUp _settingsPopUp;
        private SettingsPopUp.Factory _settingsPopUpFactory;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               IGameStateProvider gameStateProvider,
                               LevelsBlockFactory levelsBlockFactory,
                               ChipsCounter chipsCounter,
                               SettingsPopUp.Factory settingsPopUpFactory)
        {
            _settingsProvider = settingsProvider;
            _gameStateProvider = gameStateProvider;
            _levelsBlockFactory = levelsBlockFactory;
            _chipsCounter = chipsCounter;
            _settingsPopUpFactory = settingsPopUpFactory;
        }

        public void BindViews()
        {
            _chipsCounter.BindView(_chipsCounterView);
        }

        public void InitChips()
        {
            _chipsCounter.InitChips();
        }

        public void OpenSettings()
        {
            if (_settingsPopUp == null)
                _settingsPopUp = _settingsPopUpFactory.Create();

            _settingsPopUp.Open();
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
            //int lastPassedLevelNumber = _gameStateProvider.GameState.LastPassedLevelNumber.Value;
            int lastPassedLevelNumber = 7;

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

                CreateLevelsBlock(levelNumberRange, progress);
            }
        }

        private void CreateLevelsBlock(Vector2Int levelNumberRange, float progress)
        {
            var levelsBlock = _levelsBlockFactory.Create(levelNumberRange, progress);
            levelsBlock.Attach(_levelBlcoksContainer);
            levelsBlock.CreateLevelButtons(levelNumberRange, progress, this);
        }
    }
}
