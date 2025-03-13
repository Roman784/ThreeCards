using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LevelMenu
{
    public class LevelsBlockView : MonoBehaviour
    {
        [SerializeField] private Transform _levelsContainer;
        [SerializeField] private TMP_Text _levelNumberRangeView;

        [Space]

        [SerializeField] private Image _progressBarView;
        [SerializeField] private float _progressBarFillingDuration;

        [Space]

        [SerializeField] private Transform _openIconView;
        [SerializeField] private float _openIconRotationDuration;

        [Space]

        [SerializeField] private RectTransform _mask;
        [SerializeField] private float _openingDuration;
        [SerializeField] private float _closingDuration;
        [SerializeField] private float _openingHeinght;
        private float _closingHeight;

        private LevelButtonFactory _levelButtonFactory;

        public event Action OnOpenClose;

        [Inject]
        private void Construct(LevelButtonFactory levelButtonFactory)
        {
            _levelButtonFactory = levelButtonFactory;
        }

        private void Awake()
        {
            _closingHeight = _mask.sizeDelta.y;
        }

        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetLevelNumberRange(Vector2Int levelNumberRange)
        {
            _levelNumberRangeView.text = $"{levelNumberRange.x} - {levelNumberRange.y}";
        }

        public void SetProgress(float progress)
        {
            _progressBarView.DOFillAmount(progress, _progressBarFillingDuration)
                .SetEase(Ease.InOutQuad);
        }

        public void OpenClose()
        {
            OnOpenClose?.Invoke();
        }

        public void Open()
        {
            _mask.DOSizeDelta(new Vector2(_mask.sizeDelta.x, _openingHeinght), _openingDuration)
                .SetEase(Ease.OutBack);

            _openIconView.DORotate(new Vector3(0, 0, -90), _openIconRotationDuration)
                .SetEase(Ease.OutBounce);
        }

        public void Close()
        {
            _mask.DOSizeDelta(new Vector2(_mask.sizeDelta.x, _closingHeight), _closingDuration)
                .SetEase(Ease.OutQuad);

            _openIconView.DORotate(new Vector3(0, 0, 0), _openIconRotationDuration)
                .SetEase(Ease.OutBounce);
        }

        public LevelButton CreateLevelButton(int number, bool isPassed, bool isLocked, LevelMenuUI levelMenu)
        {
            return _levelButtonFactory.Create(number, isPassed, isLocked, levelMenu);
        }

        public void AttachButton(LevelButton button)
        {
            button.Attach(_levelsContainer);
        }
    }
}
