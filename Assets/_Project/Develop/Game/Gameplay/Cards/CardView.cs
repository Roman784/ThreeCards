using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        private Sprite _faceSprite;
        private Sprite _backSprite;

        private Dictionary<Suits, Sprite> _spritesMap = new();

        [HideInInspector] public UnityEvent OnPicked = new();

        private void Awake()
        {
            _spritesMap[Suits.Diamonds] = _diamonds;
            _spritesMap[Suits.Heart] = _hearts;
            _spritesMap[Suits.Club] = _clubs;
            _spritesMap[Suits.Spade] = _spades;
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

        public void Move(Transform slot)
        {
            transform.localScale = slot.localScale;
            transform.position = slot.position;
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
    }
}
