using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace Gameplay
{
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

        private Sprite _faceSprite;
        private Sprite _backSprite;

        private Dictionary<Suits, Sprite> _spritesMap = new();

        private Coroutine _moveRoutine;

        [HideInInspector] public UnityEvent OnPicked = new();

        private void Awake()
        {
            _spritesMap[Suits.Diamonds] = _diamonds;
            _spritesMap[Suits.Heart] = _hearts;
            _spritesMap[Suits.Club] = _clubs;
            _spritesMap[Suits.Spade] = _spades;
        }

        private void OnDestroy()
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);
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

        public void Place(Transform slot)
        {
            transform.SetParent(slot);
            _moveRoutine = StartCoroutine(MoveRoutine(slot.position));
        }

        public void Close(bool instantly = false)
        {
            if (instantly)
            {
                _spriteView.sprite = _backSprite;
                _rankView.gameObject.SetActive(false);
                _raycaster.enabled = false;
            }
        }

        public void Open()
        {
            _spriteView.sprite = _faceSprite;
            _rankView.gameObject.SetActive(true);
            _raycaster.enabled = true;
        }

        private IEnumerator MoveRoutine(Vector2 targetPosition)
        {
            while (Vector2.Distance(transform.position, targetPosition) > 0.05f)
            {
                yield return null;
                transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
            }

            transform.position = targetPosition;
        }
    }
}
