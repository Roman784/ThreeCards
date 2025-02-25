using System.Collections.Generic;
using System;
using Gameplay;
using GameplayServices;
using R3;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using System.IO;

namespace GameState
{
    public class GameSessionStateProvider
    {
        private List<GameSessionState> _states = new();

        private Card[,] _cardsMap;
        private List<Slot> _slots;

        public GameSessionStateProvider(Card[,] cardsMap, List<Slot> slots, 
                                        CardPlacingService cardPlacingService, CardMatchingService cardMatchingService)
        {
            _cardsMap = cardsMap;
            _slots = slots;

            cardPlacingService.OnCardPlaced.Subscribe(_ => CreateState());
            cardMatchingService.OnCardsRemoved.Subscribe(_ => CreateState());
        }

        public GameSessionState GetLastState()
        {
            if (_states.Count == 0)
                throw new NullReferenceException("The game session state list is empty.");

            return _states[_states.Count - 1];
        }

        private void CreateState()
        {
            var gameSessionState = CreateGameSessionState();
            _states.Add(gameSessionState);

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
