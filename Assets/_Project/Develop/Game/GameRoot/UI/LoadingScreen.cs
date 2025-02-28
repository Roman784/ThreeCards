using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panel;
        [SerializeField] private float _duration;

        public Observable<Unit> Show()
        {
            var completedSubj = new Subject<Unit>();

            _panel.gameObject.SetActive(true);
            _panel.alpha = 0f;
            _panel.DOFade(1f, _duration).SetEase(Ease.OutExpo).OnComplete(() => 
            {
                completedSubj.OnNext(Unit.Default);
                completedSubj.OnCompleted();
            });

            return completedSubj;
        }

        public Observable<Unit> Hide()
        {
            var completedSubj = new Subject<Unit>();

            _panel.gameObject.SetActive(true);
            _panel.alpha = 1f;
            _panel.DOFade(0f, _duration).SetEase(Ease.InExpo).OnComplete(() =>
            {
                completedSubj.OnNext(Unit.Default);
                completedSubj.OnCompleted();
            });

            return completedSubj;
        }
    }
}
