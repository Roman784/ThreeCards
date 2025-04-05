using Audio;
using Currencies;
using GameplayRoot;
using GameplayServices;
using GameRoot;
using GameState;
using LevelMenuRoot;
using R3;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SceneUI : MonoBehaviour
    {
        [SerializeField] private ChipsCounterView _chipsCounterView;

        protected ISettingsProvider _settingsProvider;
        protected IGameStateProvider _gameStateProvider;
        protected PopUpProvider _popUpProvider;
        protected AudioPlayer _audioPlayer;
        protected ChipsCounter _chipsCounter;

        protected UIAudioSettings AudioSettings => _settingsProvider.GameSettings.AudioSettings.UIAudioSettings;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               IGameStateProvider gameStateProvider,
                               ChipsCounter chipsCounter,
                               PopUpProvider popUpProvider,
                               AudioPlayer audioPlayer)
        {
            _settingsProvider = settingsProvider;
            _gameStateProvider = gameStateProvider;
            _chipsCounter = chipsCounter;
            _popUpProvider = popUpProvider;
            _audioPlayer = audioPlayer;
        }

        public virtual void BindViews()
        {
            _chipsCounter.BindView(_chipsCounterView);
        }

        public void InitChips(Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved = null)
        {
            _chipsCounter.InitChips(onCardsRemoved);
        }

        public void OpenSettings()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenSettingsPopUp();
        }

        protected void OpenLevel(int number, float bonusWhirlpoolTimerValue)
        {
            var gameplayEnterParams = new GameplayEnterParams(number, bonusWhirlpoolTimerValue);
            new SceneLoader().LoadAndRunGameplay(gameplayEnterParams);
        }

        protected void PlayButtonClickSound()
        {
            _audioPlayer.PlayOneShot(AudioSettings.ButtonClickSound);
        }
    }
}
