using BonusWhirlpool;
using Gameplay;
using GameplayServices;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using R3;
using System.Linq;

namespace BonusWhirlpoolService
{
    public class CardWhirlpoolService
    {
        private CardFactory _cardFactory;
        private CardWhirlpoolSettings _settings;
        private CardPlacingService _cardPlacingService;
        private CardMarkingService _cardMarkingService;

        private List<WhirlpoolCard> _cards = new();

        public CardWhirlpoolService(CardFactory cardFactory, CardWhirlpoolSettings settings, 
                                    CardPlacingService cardPlacingService, CardMarkingService cardMarkingService)
        {
            _cardFactory = cardFactory;
            _settings = settings;
            _cardPlacingService = cardPlacingService;
            _cardMarkingService = cardMarkingService;

            _cardPlacingService.OnCardReadyToPlaced.Subscribe(card => RemoveCard(card));
        }

        public List<WhirlpoolCard> Start()
        {
            CreateCards();
            Coroutines.StartRoutine(MoveCards());

            return _cards;
        }

        private void CreateCards()
        {
            _cards.Capacity = _settings.Count;

            for (int i = 0; i < _settings.Count; i++)
            {
                CreateCard();
            }
        }

        private void CreateCard()
        {
            var card = _cardFactory.Create();

            card.SetPlacingService(_cardPlacingService);
            _cardMarkingService.MarkRandom(card);

            card.Open();
            card.Disable(false);

            var radius = Randomizer.GetRandomRange(_settings.Radius, _settings.RadiusOffset);
            var flightSpeed = Randomizer.GetRandomRange(_settings.FlightSpeed, _settings.FlightSpeedOffset);
            var trajectoryAngleOffset = Randomizer.GetRandomRange(0, _settings.TrajectoryAngleOffset);
            var rotationSpeed = Randomizer.GetRandomRange(_settings.RotationSpeed, _settings.RotationSpeedOffset);

            var whirlpoolCard = new WhirlpoolCard(card, radius, flightSpeed, trajectoryAngleOffset, 
                                                  rotationSpeed, _settings.PositionOffset);
            _cards.Add(whirlpoolCard);
        }

        private IEnumerator MoveCards()
        {
            while (_cards.Count > 0)
            {
                foreach (var card in _cards)
                {
                    card.Move();
                }

                yield return null;
            }
        }

        private void RemoveCard(Card card)
        {
            var whirlpoolCard = _cards.FirstOrDefault(whirlpoolCard => whirlpoolCard.Card == card);
            if (whirlpoolCard == null) return;

            _cards.Remove(whirlpoolCard);
            CreateCard();
        }
    }
}
