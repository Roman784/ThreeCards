using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScriptAnimations
{
    public class ZoomOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _zoomScale;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = _target.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _target.DOScale(_initialScale * _zoomScale, _duration).SetEase(_ease);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _target.DOScale(_initialScale, _duration).SetEase(_ease);
        }
    }
}
