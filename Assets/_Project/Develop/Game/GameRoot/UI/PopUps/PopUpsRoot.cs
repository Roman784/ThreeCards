using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopUpsRoot : MonoBehaviour
    {
        [SerializeField] private Transform _popUpsContainer;

        [Space]

        [SerializeField] private Image _fadeView;
        [SerializeField] private float _fadeValue;
        [SerializeField] private float _fadeDuration;

        private Stack<PopUp> _popUps = new();
        private bool _isFaded;

        public PopUp LastPopUp => _popUps.Count > 0 ? _popUps.Peek() : null;

        private void Awake()
        {
            _isFaded = false;
        }

        public void AttachPopUp(PopUp popUp)
        {
            popUp.transform.SetParent(_popUpsContainer, false);
        }

        public void DestroyAllPopUps()
        {
            foreach (var popUp in _popUps)
            {
                popUp.Destroy();
            }

            _popUps = new();
        }

        public void PushPopUp(PopUp popUp)
        {
            _popUps.Push(popUp);
        }

        public void RemoveLastPopUp()
        {
            if (_popUps.Count > 0)
                _popUps.Pop();
        }

        public void CloseLastPopUp()
        {
            LastPopUp?.Close();
        }

        public void FadeScreen()
        {
            if (_isFaded) return;
            _isFaded = true;

            _fadeView.gameObject.SetActive(true);
            _fadeView.DOFade(0f, 0f);
            _fadeView.DOFade(_fadeValue, _fadeDuration).SetEase(Ease.OutQuad);
        }

        public void AppearScreen()
        {
            if (!_isFaded) return;
            _isFaded = false;

            _fadeView.DOFade(_fadeValue, 0f);
            _fadeView.DOFade(0, _fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _fadeView.gameObject.SetActive(false);
            });
        }
    }
}
