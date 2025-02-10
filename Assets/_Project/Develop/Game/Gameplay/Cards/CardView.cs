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

        private Coroutine _moveRoutine;
        private Coroutine _flipRoutine;

        private Animator _animator;

        [HideInInspector] public UnityEvent OnPicked = new();

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
            StopAllRoutines();
        }

        public void Mark(Suits suit, Ranks rank)
        {
            _faceSprite = _spritesMap[suit];

            if (suit is Suits.Heart or Suits.Diamonds)
                _backSprite = _redCardBack;
            else
                _backSprite = _blackCardBack;

            _spriteView.sprite = _faceSprite;
            _rankView.text = CardMarkingMapper.RanksMap[rank];
        }

        public void Pick()
        {
            OnPicked.Invoke();
        }

        public Observable<Unit> Place(Transform slot)
        {
            transform.SetParent(slot);

            var moveCompletedSubject = new Subject<Unit>();
            _moveRoutine = StartCoroutine(MoveRoutine(slot.position, moveCompletedSubject));

            return moveCompletedSubject;
        }

        public void Close(bool instantly = false)
        {
            _raycaster.enabled = false;

            if (instantly)
            {
                _spriteView.sprite = _backSprite;
                _rankView.gameObject.SetActive(false);
                return;
            }

            _flipRoutine = StartCoroutine(FlipRoutine(1, -1, _backSprite, false));
        }

        public void Open()
        {
            _raycaster.enabled = true;
            _flipRoutine = StartCoroutine(FlipRoutine(-1, 1, _faceSprite, true));
        }

        public Observable<Unit> Destroy()
        {
            var animationCompletedSubject = new Subject<Unit>();
            Coroutines.StartRoutine(DestroyRoutine(animationCompletedSubject));

            return animationCompletedSubject;
        }

        private IEnumerator MoveRoutine(Vector3 targetPosition, Subject<Unit> moveCompletedSubject)
        {
            while (Vector2.Distance(transform.position, targetPosition) > 0.03f)
            {
                transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
                yield return null;
            }

            transform.position = targetPosition;

            moveCompletedSubject.OnNext(Unit.Default);
            moveCompletedSubject.OnCompleted();
        }

        private IEnumerator FlipRoutine(float from, float to, Sprite sprite, bool rankActive)
        {
            transform.localScale = new Vector2(from, 1f);
            
            yield return RotateRoutine(0, Vector2.MoveTowards);

            _spriteView.sprite = sprite;
            _rankView.gameObject.SetActive(rankActive);

            yield return RotateRoutine(to, Vector2.Lerp);
        }

        private IEnumerator RotateRoutine(float to, Func<Vector2, Vector2, float, Vector2> action)
        {
            Vector2 targetScale = new Vector2(to, 1f);

            while (math.abs(to - transform.localScale.x) > 0.03f)
            {
                transform.localScale = action(transform.localScale, targetScale, Time.deltaTime * _flipSpeed);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        private IEnumerator DestroyRoutine(Subject<Unit> animationCompletedSubject)
        {
            _animator.SetTrigger("Destroying");

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

            animationCompletedSubject.OnNext(Unit.Default);
            animationCompletedSubject.OnCompleted();
        }

        private void StopAllRoutines()
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            if (_flipRoutine != null)
                StopCoroutine(_flipRoutine);
        }
    }
}
