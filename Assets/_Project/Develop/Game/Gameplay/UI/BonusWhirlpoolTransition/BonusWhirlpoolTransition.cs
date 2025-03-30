using Utils;
using R3;
using UnityEngine;
using GameplayRoot;

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

        private float FillingProgress => 1f - _currentTimerValue / _timerValue;

        public BonusWhirlpoolTransition(float timerValue, float currentTimerValue)
        {
            _timerValue = timerValue;
            _currentTimerValue = currentTimerValue;
        }

        public void Init(GameplayPopUpProvider popUpProvider, GameplayEnterParams gameplayEnterParams)
        {
            _popUpProvider = popUpProvider;
            _gameplayEnterParams = gameplayEnterParams;
        }

        public void StartTimer()
        { 
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
            _currentTimerValue = value;
            _view?.SetProgress(FillingProgress);
        }

        private void OpenPopUp()
        {
            _popUpProvider.OpenBonusWhirlpoolTransitionPopUp(_gameplayEnterParams, _timer);
        }
    }
}
