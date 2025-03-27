using R3;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class Timer
    {
        private ReactiveProperty<float> _value;
        private TimerView _view;

        public Timer(float value)
        {
            _value = new(value);
        }

        public void BindView(TimerView view)
        {
            _view = view;
            _value.Subscribe(value => _view.Render(value));
        }

        public Observable<Unit> Start()
        {
            var completedSubj = new Subject<Unit>();
            Coroutines.StartRoutine(CountDown(completedSubj));

            return completedSubj;
        }

        private IEnumerator CountDown(Subject<Unit> completedSubj)
        {
            while (_value.Value > 0)
            {
                _value.Value -= Time.deltaTime;
                yield return null;
            }

            _value.Value = 0;

            completedSubj.OnNext(Unit.Default);
            completedSubj.OnCompleted();
        }
    }
}
