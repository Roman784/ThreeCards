using System.Collections.Generic;
using System;
using Gameplay;
using GameplayServices;
using R3;
using UnityEngine;
using System.IO;
using Utils;

namespace GameState
{
    public class GameSessionStateProvider
    {
        private Stack<GameSessionState> _states = new();

        private Card[,] _cardsMap;
        private List<Slot> _slots;
        private CardPlacingService _cardPlacingService;
        private CardMatchingService _cardMatchingService;
        private CardFactory _cardFactory;

        public GameSessionStateProvider(Card[,] cardsMap, List<Slot> slots, 
                                        CardPlacingService cardPlacingService, CardMatchingService cardMatchingService,
                                        CardFactory cardFactory)
        {
            _cardsMap = cardsMap;
            _slots = slots;
            _cardPlacingService = cardPlacingService;
            _cardMatchingService = cardMatchingService;
            _cardFactory = cardFactory;

            _cardPlacingService.OnCardPlaced.Subscribe(_ => CreateState());
            _cardMatchingService.OnCardsRemoved.Subscribe(_ => CreateState());
        }

        public void SetLastState()
        {
            /*var state = GetLastState();

            foreach (var slot in _slots)
            {
                slot.RemoveCard();
            }

            foreach (var card in _cardsMap)
            {
                card?.Destroy();
            }

            var cardLayoutService = new CardLayoutService(layouts, _cardFactory, _cardPlacingService);
            var cardMarkingService = new CardMarkingService();

            var cardsMap = cardLayoutService.SetUp(layout);
            cardMarkingService.Mark(cardsMap, layout.CardSpreadRange);

            // Animations.
            var cardFlippingService = new CardFlippingService(cardsMap, _cardPlacingService);
            var fieldAnimationService = new FieldAnimationService(cardsMap, cardFlippingService);
            fieldAnimationService.LayOutCards();*/
        }

        private GameSessionState GetLastState()
        {
            if (_states.Count == 0)
                throw new NullReferenceException("The game session state list is empty.");

            return _states.Peek();
        }

        private void CreateState()
        {
            var gameSessionState = CreateGameSessionState();
            _states.Push(gameSessionState);

            // <- Тест.
            var path = Path.Combine(Application.dataPath, "gameSession.json");
            string json = JsonUtility.ToJson(gameSessionState, true);
            File.WriteAllText(path, json);
        }

        private GameSessionState CreateGameSessionState()
        {
            return new GameSessionState()
            {
                CardLayoutEntity = CreateCardLayoutEntity(),
                SlotsEntity = CreateSlotsEntity(),
            };
        }

        private CardLayoutEntity CreateCardLayoutEntity()
        {
            var cards = new List<CardEntity>();

            for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
            {
                for (int columnI = 0; columnI < _cardsMap.GetLength(0); columnI++)
                {
                    var card = _cardsMap[columnI, cardI];
                    if (card == null) continue;

                    cards.Add(CreateCardEntity(card, columnI, cardI));
                }
            }

            return new CardLayoutEntity()
            {
                Cards = cards,
            };
        }

        private SlotsEntity CreateSlotsEntity()
        {
            var slots = new List<SlotEntity>();

            foreach (var slot in _slots)
            {
                if (slot.Card == null) continue;
                slots.Add(CreateSlotEntity(slot));
            }

            return new SlotsEntity()
            {
                Slots = slots,
            };
        }

        private SlotEntity CreateSlotEntity(Slot slot)
        {
            return new SlotEntity()
            {
                Card = CreateCardEntity(slot.Card, 0, 0),
        };
        }

        private CardEntity CreateCardEntity(Card card, int columnIndex, int rowIndex)
        {
            return new CardEntity()
            {
                Rank = CardMarkingMapper.GetRankIndex(card.Rank),
                Suit = CardMarkingMapper.GetSuitIndex(card.Suit),
                ColumnIndex = columnIndex,
                RowIndex = rowIndex,
            };
        }
    }
}
