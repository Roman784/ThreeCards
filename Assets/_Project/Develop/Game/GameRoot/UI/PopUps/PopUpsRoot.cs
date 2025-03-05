using DG.Tweening;
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

        public void AttachPopUp(PopUp popUp)
        {
            popUp.transform.SetParent(_popUpsContainer, false);
        }

        public void FadeScreen()
        {
            _fadeView.gameObject.SetActive(true);
            _fadeView.DOFade(0f, 0f);
            _fadeView.DOFade(_fadeValue, _fadeDuration).SetEase(Ease.OutQuad);
        }

        public void AppearScreen()
        {
            _fadeView.DOFade(_fadeValue, 0f);
            _fadeView.DOFade(0, _fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _fadeView.gameObject.SetActive(false);
            });
        }
    }
}
