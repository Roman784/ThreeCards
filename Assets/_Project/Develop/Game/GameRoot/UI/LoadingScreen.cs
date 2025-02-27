using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image[] _panels;
        [SerializeField] private float _duration;

        public Observable<Unit> Show()
        {
            var completedSubj = new Subject<Unit>();
            var panel = GetRandomPanel();

            panel.gameObject.SetActive(true);
            DeactiveAllPanels(panel.gameObject);

            panel.transform.localScale = Vector3.zero;
            var color = panel.color;
            color.a = 0f;
            panel.color = color;

            panel.transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutQuad);
            panel.DOFade(1f, _duration / 1.2f).SetEase(Ease.Flash);

            DOVirtual.DelayedCall(_duration, () =>
            {
                completedSubj.OnNext(Unit.Default);
                completedSubj.OnCompleted();
            });

            return completedSubj;
        }

        public Observable<Unit> Hide()
        {
            var completedSubj = new Subject<Unit>();
            var panel = GetRandomPanel();

            panel.gameObject.SetActive(true);
            DeactiveAllPanels(panel.gameObject);

            panel.transform.localScale = Vector3.one;
            var color = panel.color;
            color.a = 1f;
            panel.color = color;

            panel.transform.DOScale(Vector3.zero, _duration).SetEase(Ease.InQuad);
            panel.DOFade(0f, _duration).SetEase(Ease.Flash);

            DOVirtual.DelayedCall(_duration, () =>
            {
                panel.gameObject.SetActive(false);
                completedSubj.OnNext(Unit.Default);
                completedSubj.OnCompleted();
            });

            return completedSubj;
        }

        private Image GetRandomPanel()
        {
            return Randomizer.GetRandomValue(_panels);
        }

        private void DeactiveAllPanels(GameObject without = null)
        {
            foreach (var panel in _panels)
            {
                if (panel.gameObject != without)
                    panel.gameObject.SetActive(false);
            }
        }
    }
}
