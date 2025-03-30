using Utils;
using R3;
using UnityEngine;

namespace UI
{
    public class BonusWhirlpoolTransition
    {
        private BonusWhirlpoolTransitionView _view;
        private float _timerValue;
        private float _currentTimerValue;

        public BonusWhirlpoolTransition(float timerValue, float currentTimerValue)
        {
            _timerValue = timerValue;
            _currentTimerValue = currentTimerValue;

            var timer = new Timer(timerValue - currentTimerValue);
            timer.OnValueChanged.Subscribe(value => ChangeCurrentValue(value));

            timer.Start();
        }

        public void BindView(BonusWhirlpoolTransitionView view)
        {
            _view = view;
        }

        private void ChangeCurrentValue(float value)
        {
            _currentTimerValue = value;

            var fillingProgress = 1f - _currentTimerValue / _timerValue;
            _view?.SetProgress(fillingProgress);
        }
    }
}
