using Utils;
using R3;
using UnityEngine;
using GameplayRoot;
using Audio;
using Settings;
using UnityEditor;

namespace UI
{
    public class BonusWhirlpoolTransition
    {
        private BonusWhirlpoolTransitionView _view;

        private Timer _timer;
        private float _timerValue;
        private float _currentTimerValue;

        private GameplayPopUpProvider _popUpProvider;
        private GameplayEnterParams _gameplayEnterParams;
        private AudioPlayer _audioPlayer;
        private UIAudioSettings _uiAudioSettings;

        public float CurrentTimerValue => _currentTimerValue;
        private float FillingProgress => _currentTimerValue / _timerValue;

        public void Init(GameplayPopUpProvider popUpProvider, GameplayEnterParams gameplayEnterParams,
                         AudioPlayer audioPlayer, UIAudioSettings uiAudioSettings)
        {
            _popUpProvider = popUpProvider;
            _gameplayEnterParams = gameplayEnterParams;
            _audioPlayer = audioPlayer;
            _uiAudioSettings = uiAudioSettings;
        }

        public void StartTimer(float timerValue, float currentTimerValue)
        {
            _timerValue = timerValue;
            _currentTimerValue = currentTimerValue;

            _timer = new Timer(_timerValue - _currentTimerValue);
            _timer.OnValueChanged.Subscribe(value => ChangeCurrentValue(value));

            _timer.Start();
        }

        public void StopTimer()
        {
            _timer?.Stop();
        }

        public void BindView(BonusWhirlpoolTransitionView view)
        {
            _view = view;
            _view.OnOpenPopUp += () => OpenPopUp();
        }

        private void ChangeCurrentValue(float value)
        {
            _currentTimerValue = _timerValue - value;
            _view?.SetProgress(FillingProgress);

            if (_currentTimerValue >= 1f)
                _view?.StartPulse();
        }

        private void OpenPopUp()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenBonusWhirlpoolTransitionPopUp(_gameplayEnterParams, _timer);
        }

        private void PlayButtonClickSound()
        {
            _audioPlayer.PlayOneShot(_uiAudioSettings.ButtonClickSound);
        }
    }
}
