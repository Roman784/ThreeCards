using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptAnimations
{
    public class FadeImage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _duration;
        [SerializeField, Range(0f, 1f)] private float _fadeValue;

        private float _initialFadeValue;
        private ZoomOnHover _zoomOnHover;

        private void Awake()
        {
            _initialFadeValue = _image.color.a;

            if (TryGetComponent<ZoomOnHover>(out ZoomOnHover zoomOnHover))
                _zoomOnHover = zoomOnHover;
        }

        public void FadeIn()
        {
            _image.DOFade(_initialFadeValue, _duration).SetEase(Ease.InOutSine);

            if (_zoomOnHover != null)
                _zoomOnHover.enabled = true;
        }

        public void FadeOut()
        {
            _image.DOFade(_fadeValue, _duration).SetEase(Ease.InOutSine);

            if (_zoomOnHover != null)
                _zoomOnHover.enabled = false;
        }
    }
}
