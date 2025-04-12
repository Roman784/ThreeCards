using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScriptAnimations
{
    public class FillOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector2 _fillValue = new Vector2(100, 100);
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private Ease _ease = Ease.OutBack;

        private Vector2 _initialFill;
        private Tween _currentTween;

        private void Awake()
        {
            _initialFill = _target.sizeDelta;
        }

        private void OnDisable()
        {
            _target.sizeDelta = _initialFill;
            _currentTween?.Kill();
        }

        private void Update()
        {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            if (UnityEngine.Device.Application.isMobilePlatform && 
                Input.touchCount == 0 && _target.sizeDelta != _initialFill)
            {
                ZoomOut();
            }
#endif
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ZoomIn();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ZoomOut();
        }

        public void ZoomIn()
        {
            _currentTween?.Kill();
            _currentTween = _target.DOSizeDelta(_fillValue, _duration).SetEase(_ease);
        }

        public void ZoomOut()
        {
            _currentTween?.Kill();
            _currentTween = _target.DOSizeDelta(_initialFill, _duration).SetEase(_ease);
        }
    }
}
