using DG.Tweening;
using R3;
using UnityEngine;

namespace UI
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private float _rotationDuration = 0.2f;

        private PopUpsRoot _root;
        private PopUp _previousPopUp;

        private void Awake()
        {
            _view.gameObject.SetActive(false);
        }

        public void Init(PopUpsRoot root, PopUp previousPopUp = null)
        {
            _root = root;
            _previousPopUp = previousPopUp;
        }

        public virtual void Open()
        {
            _root.PushPopUp(this);

            if (_previousPopUp == null)
                _root.FadeScreen();

            (_previousPopUp?.RotateToClose() ?? Observable.Return(Unit.Default)).Subscribe(_ =>
            {
                _view.gameObject.SetActive(true);
                RotateToOpen();
            });
        }

        public virtual void Close()
        {
            _root.RemoveLastPopUp();

            if (_previousPopUp == null)
                _root.AppearScreen();

            RotateToClose().Subscribe(_ =>
            {
                _view.gameObject.SetActive(false);

                if (_previousPopUp != null)
                    _previousPopUp.RotateToOpen();
            });
        }

        public Observable<Unit> RotateToOpen()
        {
            var from = new Vector3(0, -90, 0);
            var to = new Vector3(0, 0, 0);

            return Rotate(from, to);
        }

        public Observable<Unit> RotateToClose()
        {
            var from = new Vector3(0, 0, 0);
            var to = new Vector3(0, -90, 0);

            return Rotate(from, to);
        }

        private Observable<Unit> Rotate(Vector3 from, Vector3 to)
        {
            var onRotated = new Subject<Unit>();

            _view.rotation = Quaternion.Euler(from);
            _view.DORotate(to, _rotationDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                onRotated.OnNext(Unit.Default);
                onRotated.OnCompleted();
            });

            return onRotated;
        }
    }
}
