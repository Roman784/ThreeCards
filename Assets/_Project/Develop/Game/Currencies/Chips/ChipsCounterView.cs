using System.Collections;
using TMPro;
using UnityEngine;
using Utils;

namespace Currencies
{
    public class ChipsCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countView;
        private int _currentCount;

        private Coroutine _increaseCounterRoutine;

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

        private IEnumerator IncreaseCounterRoutine(int count)
        {
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
