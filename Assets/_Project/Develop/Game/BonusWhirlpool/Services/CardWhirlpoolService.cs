using BonusWhirlpool;
using Gameplay;
using GameplayServices;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace BonusWhirlpoolService
{
    public class CardWhirlpoolService
    {
        private CardFactory _cardFactory;
        private CardWhirlpoolSettings _settings;

        private List<WhirlpoolCard> _cards = new();

        public CardWhirlpoolService(CardFactory cardFactory, CardWhirlpoolSettings settings)
        {
            _cardFactory = cardFactory;
            _settings = settings;
        }

        public void Start()
        {
            CreateCards();
            Coroutines.StartRoutine(MoveCards());
        }

        private void CreateCards()
        {
            _cards.Capacity = _settings.Count;

            for (int i = 0; i < _settings.Count; i++)
            {
                var card = _cardFactory.Create();
                var radius = Randomizer.GetRandomRange(_settings.Radius, _settings.RadiusOffset);
                var flightSpeed = Randomizer.GetRandomRange(_settings.FlightSpeed, _settings.FlightSpeedOffset);
                var trajectoryAngleOffset = Randomizer.GetRandomRange(0, _settings.TrajectoryAngleOffset);
                var rotationSpeed = Randomizer.GetRandomRange(_settings.RotationSpeed, _settings.RotationSpeedOffset);

                var whirlpoolCard = new WhirlpoolCard(card, radius, flightSpeed, trajectoryAngleOffset, rotationSpeed);
                _cards.Add(whirlpoolCard);

                whirlpoolCard.OnCardPlaced += (card) => RemoveCard(card);
            }
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

        private void RemoveCard(WhirlpoolCard card)
        {
            _cards.Remove(card);
        }
    }
}
