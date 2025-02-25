using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNumberView;
        [SerializeField] private Image _fillBarView;

        [Space]

        [SerializeField] private float _barFillingSpeed;

        private Coroutine _progressBarFillingRoutine;

        private void Awake()
        {
            _fillBarView.fillAmount = 0f;
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumberView.text = levelNumber.ToString();
        }

        public void FillProgressBar(float progress)
        {
            if (_progressBarFillingRoutine != null)
                StopCoroutine(_progressBarFillingRoutine);

            _progressBarFillingRoutine = StartCoroutine(FillProgressBarRoutine(progress));
        }

        private IEnumerator FillProgressBarRoutine(float progress)
        {
            while (progress - _fillBarView.fillAmount > 0.01f)
            {
                _fillBarView.fillAmount = Mathf.MoveTowards(_fillBarView.fillAmount, progress, _barFillingSpeed * Time.deltaTime);
                yield return null;
            }
            _fillBarView.fillAmount = progress;
        }
    }
}
