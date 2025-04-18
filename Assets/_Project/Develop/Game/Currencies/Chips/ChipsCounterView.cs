using ScriptAnimations;
using System.Collections;
using TMPro;
using UnityEngine;
using Utils;
using R3;
using System;

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

        private Camera _camera;

        public event Action OnGetAdvertisingChips;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void SetCurrentCount(int count)
        {
            _currentCount = count;
            UpdateView();
        }

        public void ChangeCounter(int count)
        {
            if (_increaseCounterRoutine != null)
                Coroutines.StopRoutine(_increaseCounterRoutine);

            _increaseCounterRoutine = Coroutines.StartRoutine(ChangeCounterRoutine(count));
        }

        public void GetAdvertisingChips()
        {
            OnGetAdvertisingChips?.Invoke();
        }

        public Observable<Unit> AnimateCollection(int count, Vector3 from)
        {
            from = _camera.WorldToScreenPoint(from);
            count = count / 3;

            var onCollected = _collectionAnimation.StartCollecting(count, from, _chipsIcon.position);
            onCollected.Subscribe(_ => _pulsationAnimator.Pulse(_chipsIcon));
            return onCollected;
        }

        private IEnumerator ChangeCounterRoutine(int count)
        {
            int step = Mathf.Abs(_currentCount - count) % 10;
            step = Mathf.Clamp(step, 1, step);

            while (_currentCount != count)
            {
                _currentCount = (int)Mathf.MoveTowards(_currentCount, count, 10);
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
            if (this == null) return;

            Vector2 size = _countView.rectTransform.sizeDelta;
            size.x = _countView.preferredWidth;
            _countView.rectTransform.sizeDelta = size;
        }
    }
}
