using ScriptAnimations;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils;

namespace Currencies
{
    public class ChipsCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countView;
        [SerializeField] private RectTransform _chipsIcon;
        [SerializeField] private CurrencyCollectionAnimation _collectionAnimation;
        [SerializeField] private PulsationAnimator _pulsationAnimator;

        private int _currentCount;
        private Coroutine _increaseCounterRoutine;
        private bool _canIncrease;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;

            _collectionAnimation.OnCollected.AddListener(() => _pulsationAnimator.Pulse(_chipsIcon));
            _collectionAnimation.OnCollected.AddListener(() => _canIncrease = true);
            _collectionAnimation.OnAllCollected.AddListener(() => _canIncrease = false);
        }

        public void SetCurrentCount(int count)
        {
            _currentCount = count;
            UpdateView();
        }

        public void IncreaseCounter(int count)
        {
            if (_increaseCounterRoutine != null)
                Coroutines.StopRoutine(_increaseCounterRoutine);

            _increaseCounterRoutine = Coroutines.StartRoutine(IncreaseCounterRoutine(count));
        }

        public void AnimateCollection(int count, Vector3 from)
        {
            from = _camera.WorldToScreenPoint(from);
            count = count / 3;

            _collectionAnimation.StartCollecting(count, from, _chipsIcon.position);
        }

        private IEnumerator IncreaseCounterRoutine(int count)
        {
            yield return new WaitUntil(() => _canIncrease);

            while (_currentCount < count)
            {
                _currentCount += 1;
                UpdateView();

                yield return new WaitForSeconds(0.01f);
            }
        }

        private void UpdateView()
        {
            _countView.text = _currentCount.ToString();
            ResizeView();
        }

        private void ResizeView()
        {
            Vector2 size = _countView.rectTransform.sizeDelta;
            size.x = _countView.preferredWidth;
            _countView.rectTransform.sizeDelta = size;
        }
    }
}
