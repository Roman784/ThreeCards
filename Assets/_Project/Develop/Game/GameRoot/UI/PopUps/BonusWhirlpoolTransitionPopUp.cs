using BonusWhirlpoolRoot;
using GameplayRoot;
using GameRoot;
using TMPro;
using UnityEngine;
using Utils;
using R3;
using DG.Tweening;
using DG.Tweening.Core;

namespace UI
{
    public class BonusWhirlpoolTransitionPopUp : PopUp
    {
        [SerializeField] private CanvasGroup _goToViewGroup;
        [SerializeField] private CanvasGroup _timerViewGroup;
        [SerializeField] private TMP_Text _timerView;

        [Space]

        [SerializeField] private Transform _iconView;
        [SerializeField] private float _iconRotationDuration;
        [SerializeField] private Ease _iconRotationEase;

        private GameplayEnterParams _gameplayEnterParams;
        private Timer _timer;
        private CompositeDisposable _disposable = new();
        private Tweener _rotationTween;

        public override void Open(bool fadeScreen = true)
        {
            _timerViewGroup.alpha = 0f;
            _goToViewGroup.alpha = 1f;

            base.Open(fadeScreen);
        }
        public override void Close(bool appearScreen = true)
        {
            _disposable.Dispose();
            base.Close(appearScreen);
        }

        public void SetGameplayEnterParams(GameplayEnterParams enterParams)
        {
            _gameplayEnterParams = enterParams;
        }

        public void SetTimer(Timer timer)
        {
            _timer = timer;
            _timer.OnValueChanged.Subscribe(value => UpdateView(value)).AddTo(_disposable);
        }

        public void OpenBonusLevel()
        {
            if (_timer.Value > 0) return;

            var enterParams = new BonusWhirlpoolEnterParams(_gameplayEnterParams.LevelNumber);
            new SceneLoader().LoadAndRunBonusWhirlpool(enterParams);
        }

        private void UpdateView(float time)
        {
            RenderTime(time);

            if (time <= 0)
            {
                _timerViewGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);
                _goToViewGroup.DOFade(1f, 0.25f).SetEase(Ease.InQuad).OnComplete(() => RotateIcon());
            }
            else
            {
                _timerViewGroup.alpha = 1f;
                _goToViewGroup.alpha = 0f;
            }
        }

        private void RenderTime(float time)
        {
            var minutes = (int)(time / 60f);
            var secs = (int)(time % 60f);

            var formattedTime = string.Format("{0:00}:{1:00}",
                minutes,
                secs);

            _timerView.text = formattedTime;
        }

        private void RotateIcon()
        {
            _rotationTween?.Kill();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _rotationTween = _iconView.DORotate(new Vector3(0f, 0f, -360f), _iconRotationDuration, RotateMode.FastBeyond360)
                                    .SetEase(_iconRotationEase)
                                    .OnComplete(() => RotateIcon());
            });
        }

        public class Factory : PopUpFactory<BonusWhirlpoolTransitionPopUp>
        {
            public BonusWhirlpoolTransitionPopUp Create(GameplayEnterParams gameplayEnterParams, Timer timer)
            {
                var popUp = base.Create();
                popUp.SetGameplayEnterParams(gameplayEnterParams);
                popUp.SetTimer(timer);

                return popUp;
            }
        }
    }
}
