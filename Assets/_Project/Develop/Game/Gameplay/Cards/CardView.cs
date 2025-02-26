using DG.Tweening;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _spriteView;
        [SerializeField] private TMP_Text _rankView;
        [SerializeField] private GraphicRaycaster _raycaster;

        [Space]

        [SerializeField] private Sprite _diamonds;
        [SerializeField] private Sprite _hearts;
        [SerializeField] private Sprite _clubs;
        [SerializeField] private Sprite _spades;

        [Space]

        [SerializeField] private Sprite _redCardBack;
        [SerializeField] private Sprite _blackCardBack;

        [Space]

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _flipSpeed;
        [SerializeField] private float _destroyingDuration;

        private Sprite _faceSprite;
        private Sprite _backSprite;

        private Dictionary<Suits, Sprite> _spritesMap = new();

        private Animator _animator;

        public Observable<Unit> OnPicked => _pickedSubj;
        private Subject<Unit> _pickedSubj = new();

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _spritesMap[Suits.Diamonds] = _diamonds;
            _spritesMap[Suits.Heart] = _hearts;
            _spritesMap[Suits.Club] = _clubs;
            _spritesMap[Suits.Spade] = _spades;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public Vector3 GetPosition() => transform.position;
        public void SetPosition(Vector3 position) => transform.position = position;

        public void Mark(Suits suit, Ranks rank)
        {
            _faceSprite = _spritesMap[suit];

            if (suit is Suits.Heart or Suits.Diamonds)
                _backSprite = _redCardBack;
            else
                _backSprite = _blackCardBack;

            _spriteView.sprite = _faceSprite;
            _rankView.text = CardMarkingMapper.GetRankView(rank);
        }

        public void Pick()
        {
            _pickedSubj.OnNext(Unit.Default);
        }

        public Observable<Unit> Place(Transform slot)
        {
            _raycaster.enabled = false;
            transform.SetParent(slot);
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
                return null;
            }

            _animator.SetTrigger("Closing");
            return CurrentAnimationDelayedCall();
        }

        public Observable<Unit> Open()
        {
            _raycaster.enabled = true;
            _animator.SetTrigger("Opening");
            return CurrentAnimationDelayedCall();
        }

        public void SetOpenView()
        {
            _rankView.gameObject.SetActive(true);
            _spriteView.sprite = _faceSprite;
        }

        public void SetCloseView()
        {
            _rankView.gameObject.SetActive(false);
            _spriteView.sprite = _backSprite;
        }

        public void PutDown()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger("Appearing");
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public Observable<Unit> Destroy()
        {
            _animator.SetTrigger("Destroying");
            return CurrentAnimationDelayedCall();
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
    }
}
