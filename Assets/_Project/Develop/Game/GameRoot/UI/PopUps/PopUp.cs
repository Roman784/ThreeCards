using DG.Tweening;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private float _rotationDuration;

        private PopUpsRoot _root;

        public void SetRoot(PopUpsRoot root)
        {
            _root = root;
        }

        public void Open()
        {
            _root.FadeScreen();

            _view.gameObject.SetActive(true);
            _view.DORotate(new Vector3(0, -90, 0), 0);
            _view.DORotate(new Vector3(0, 0, 0), _rotationDuration).SetEase(Ease.InOutQuad);
        }

        public void Close()
        {
            _root.AppearScreen();

            _view.DORotate(new Vector3(0, 0, 0), 0);
            _view.DORotate(new Vector3(0, -90, 0), _rotationDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                _view.gameObject.SetActive(false);
            });
        }
    }
}
