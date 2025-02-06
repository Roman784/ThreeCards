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

        [Space]

        [SerializeField] private Sprite _diamonds;
        [SerializeField] private Sprite _hearts;
        [SerializeField] private Sprite _clubs;
        [SerializeField] private Sprite _spades;

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
            _spriteView.sprite = _spritesMap[suit];
            _rankView.text = CardMarkingMapper.RanksMap[rank];
        }

        public void Pick()
        {
            OnPicked.Invoke();
        }

        public void Move(Vector2 position)
        {
            transform.position = position;
        }
    }
}
