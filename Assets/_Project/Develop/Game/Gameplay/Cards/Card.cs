using GameplayServices;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image _spriteView;
        [SerializeField] private TMP_Text _rankView;

        [Space]

        [SerializeField] private Sprite _diamonds;
        [SerializeField] private Sprite _hearts;
        [SerializeField] private Sprite _clubs;
        [SerializeField] private Sprite _spades;

        private Dictionary<Suits, Sprite> _spritesMap = new();

        public bool IsInited { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }

        private CardMatchingService _cardMatchingService;

        private void Awake()
        {
            _spritesMap[Suits.Diamonds] = _diamonds;
            _spritesMap[Suits.Heart] = _hearts;
            _spritesMap[Suits.Club] = _clubs;
            _spritesMap[Suits.Spade] = _spades;
        }

        public void Init(Suits suit, Ranks rank)
        {
            IsInited = true;
            Suit = suit;
            Rank = rank;

            _spriteView.sprite = _spritesMap[suit];
            _rankView.text = CardMarkingMapper.RanksMap[rank];
        }

        public void SetMatchingService(CardMatchingService service)
        {
            _cardMatchingService = service;
        }

        public void Select()
        {
            _cardMatchingService.PlaceCard(this);
        }

        public void Move(Vector2 position)
        {
            transform.position = position;
        }
    }
}
