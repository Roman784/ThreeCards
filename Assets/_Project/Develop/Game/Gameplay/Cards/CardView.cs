using DG.Tweening;
using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _spriteView;
        [SerializeField] private TMP_Text _rankView;
        [SerializeField] private Image _suitView;
        [SerializeField] private GraphicRaycaster _raycaster;

        [Space]

        [SerializeField] private Sprite _diamonds;
        [SerializeField] private Sprite _hearts;
        [SerializeField] private Sprite _clubs;
        [SerializeField] private Sprite _spades;
        [SerializeField] private Sprite _bomb;

        [Space]

        [SerializeField] private Sprite _redCardFace;
        [SerializeField] private Sprite _blackCardFace;

        [Space]

        [SerializeField] private Sprite _redCardBack;
        [SerializeField] private Sprite _blackCardBack;

        [Space]

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _flipSpeed;

        private Sprite _faceSprite;
        private Sprite _backSprite;

        private Dictionary<Suits, Sprite> cardFacesMap = new();
        private Dictionary<Suits, Sprite> cardBacksMap = new();
        private Dictionary<Suits, Sprite> suitsMap = new();

        private Animator _animator;

        public Observable<Unit> OnPicked => _pickedSubj;
        private Subject<Unit> _pickedSubj = new();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public Vector3 GetPosition() => transform.position;
        public void SetPosition(Vector3 position)
        {
            if (this != null)
                transform.position = position;
        }

        public void Rotate(Vector3 eulers)
        {
            if (this != null)
                transform.Rotate(eulers);
        }

        public void Mark(Suits suit, Ranks rank)
        {
            _faceSprite = GetFaceSprite(suit);
            _backSprite = GetBackSprite(suit);

            _spriteView.sprite = _faceSprite;
            _rankView.text = CardMarkingMapper.GetRankView(rank);
            _suitView.sprite = GetSuitSprite(suit);
            _suitView.SetNativeSize();
        }

        public void MarkAsBomb()
        {
            _suitView.sprite = _bomb;
            _suitView.SetNativeSize();
        }

        public void Pick()
        {
            _pickedSubj.OnNext(Unit.Default);
        }

        public Observable<Unit> Place(Transform slot)
        {
            _raycaster.enabled = false;

            transform.SetParent(slot);
            StraightenRotation();
            return Move(slot.position, Ease.OutQuad);
        }

        public Observable<Unit> Move(Vector3 position, Ease ease, float moveDuration = 0, float speedMultiplyer = 1)
        {
            if (moveDuration == 0)
                moveDuration = 1f / (_moveSpeed * speedMultiplyer);

            var moveCompletedSubj = new Subject<Unit>();

            transform.DOMove(position, moveDuration).SetEase(ease).OnComplete(() =>
            {
                moveCompletedSubj.OnNext(Unit.Default);
                moveCompletedSubj.OnCompleted();
            });

            return moveCompletedSubj;
        }

        public Observable<Unit> Close(bool instantly = false)
        {
            _raycaster.enabled = false;

            if (instantly)
            {
                SetCloseView();
                return Observable.Return(Unit.Default);
            }

            _animator.SetTrigger("Closing");
            return CurrentAnimationDelayedCall();
        }

        public Observable<Unit> Open(bool instantly = false)
        {
            _raycaster.enabled = true;

            if (instantly)
            {
                SetOpenView();
                return Observable.Return(Unit.Default);
            }

            _animator.SetTrigger("Opening");
            return CurrentAnimationDelayedCall();
        }

        public void PutDown()
        {
            Enable();
            _animator.SetTrigger("Appearing");
        }

        public void Disable(bool setActive = true)
        {
            _raycaster.enabled = false;
            if (setActive)
                gameObject.SetActive(false);
        }

        public void Enable(bool setActive = true)
        {
            _raycaster.enabled = true;
            if (setActive)
                gameObject.SetActive(true);
        }

        public Observable<Unit> Explode()
        {
            _animator.SetTrigger("Explosing");
            return CurrentAnimationDelayedCall();
        }

        public Observable<Unit> Hiss()
        {
            _animator.SetTrigger("Hissing");
            return CurrentAnimationDelayedCall();
        }

        public Observable<Unit> Destroy()
        {
            _animator.SetTrigger("Destroying");
            return CurrentAnimationDelayedCall();
        }

        private void StraightenRotation()
        {
            transform.DORotate(Vector3.zero, 0.25f)
                .SetEase(Ease.OutQuad);
        }

        private Observable<Unit> CurrentAnimationDelayedCall()
        {
            var animationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            var animationCompletedSubj = new Subject<Unit>();

            DOVirtual.DelayedCall(animationDuration, () =>
            {
                animationCompletedSubj.OnNext(Unit.Default);
                animationCompletedSubj.OnCompleted();
            });

            return animationCompletedSubj;
        }

        private Sprite GetFaceSprite(Suits suit)
        {
            if (suit is Suits.Heart or Suits.Diamonds) return _redCardFace;
            return _blackCardFace;
        }

        private Sprite GetBackSprite(Suits suit)
        {
            if (suit is Suits.Heart or Suits.Diamonds) return _redCardBack;
            return _blackCardBack;
        }

        private Sprite GetSuitSprite(Suits suit)
        {
            var suitsMap = new Dictionary<Suits, Sprite>();

            suitsMap[Suits.Diamonds] = _diamonds;
            suitsMap[Suits.Heart] = _hearts;
            suitsMap[Suits.Club] = _clubs;
            suitsMap[Suits.Spade] = _spades;

            return suitsMap[suit];
        }

        // Invoked from animations.
        public void SetOpenView()
        {
            _rankView.gameObject.SetActive(true);
            _suitView.gameObject.SetActive(true);
            _spriteView.sprite = _faceSprite;
        }

        public void SetCloseView()
        {
            _rankView.gameObject.SetActive(false);
            _suitView.gameObject.SetActive(false);
            _spriteView.sprite = _backSprite;
        }

        public void RotateToRandomAngle(float duration)
        {
            var angle = transform.eulerAngles.z + Random.Range(-15f, 15f);
            transform.DORotate(new Vector3(0f, 0f, angle), duration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        }
    }
}
