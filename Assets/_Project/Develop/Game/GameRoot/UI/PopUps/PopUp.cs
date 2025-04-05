using Audio;
using DG.Tweening;
using R3;
using Settings;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private Transform _view;
        [SerializeField] private float _rotationDuration = 0.2f;

        protected ISettingsProvider _settingsProvider;
        protected AudioPlayer _audioPlayer;

        private PopUpsRoot _root;
        private PopUp _previousPopUp;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, AudioPlayer audioPlayer)
        {
            _settingsProvider = settingsProvider;
            _audioPlayer = audioPlayer;
        }

        private void Awake()
        {
            _view.gameObject.SetActive(false);
        }

        public void Init(PopUpsRoot root, PopUp previousPopUp = null)
        {
            _root = root;
            _previousPopUp = previousPopUp;
        }

        public virtual void Open(bool fadeScreen = true)
        {
            _root.PushPopUp(this);

            if (_previousPopUp == null && fadeScreen)
                _root.FadeScreen();

            (_previousPopUp?.RotateToClose() ?? Observable.Return(Unit.Default)).Subscribe(_ =>
            {
                if (_view != null)
                {
                    _view.gameObject.SetActive(true);
                    RotateToOpen();
                }
            });
        }

        public virtual void Close(bool appearScreen = true)
        {
            _root.RemoveLastPopUp();

            if (_previousPopUp == null && appearScreen)
                _root.AppearScreen();

            RotateToClose().Subscribe(_ =>
            {
                _view.gameObject.SetActive(false);

                if (_previousPopUp != null)
                    _previousPopUp.RotateToOpen();

                Destroy();
            });
        }

        public void Destroy()
        {
            if (this != null)
                Destroy(gameObject);
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

        protected void PlayButtonClickSound()
        {
            var clip = _settingsProvider.GameSettings.AudioSettings.UIAudioSettings.ButtonClickSound;
            _audioPlayer.PlayOneShot(clip);
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
