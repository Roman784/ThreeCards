using Gameplay;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GameplayServices
{
    public class CardMarkingService
    {
        private Card[,] _cardsMap;
        private Vector2Int _cardSpreadRange;

        // This is to alternate the color of the suit.
        private bool _isRedSuitOrder;

        public CardMarkingService()
        {
            _isRedSuitOrder = Randomizer.GetRandomValue(true, false);
        }

        public void Mark(Card[,] cardsMap, Vector2Int cardSpreadRange)
        {
            _cardsMap = cardsMap;
            _cardSpreadRange = cardSpreadRange;

            for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
            {
                for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
                {
                    Card card = _cardsMap[colunmI, cardI];
                    if (card == null || card.IsInited) continue;

                    Vector2Int cardCoords = new Vector2Int(colunmI, cardI);
                    InitThreeCards(card, cardCoords);
                }
            }
        }

        private void InitThreeCards(Card originCard, Vector2Int originCardCoords)
        {
            HashSet<Card> vacantCards = new();
            SetVacantCards(vacantCards, originCard, originCardCoords);

            if (vacantCards.Count < 3)
            {
                AddAbscentVacantCards(originCardCoords, vacantCards);
            }

            var shuffledVacantCards = ShuffleCards(vacantCards);
            InitCards(shuffledVacantCards);
        }

        private void InitCards(List<Card> cards)
        { 
            Suits suit = _isRedSuitOrder ? CardMarkingMapper.GetRandomRedSuits() : CardMarkingMapper.GetRandomBlackSuits();
            _isRedSuitOrder = !_isRedSuitOrder;

            for (int i = 0; i < 3; i++)
            {
                Card card = cards[i];
                Ranks rank = CardMarkingMapper.GetRandomRank();

                card.Init(suit, rank);
            }
        }

        private void SetVacantCards(HashSet<Card> vacantCards, Card originCard, Vector2Int originCardCoords)
        {
            int startX = Mathf.Clamp(originCardCoords.x - _cardSpreadRange.x, 0, originCardCoords.x);
            int endX = Mathf.Clamp(originCardCoords.x + _cardSpreadRange.x, originCardCoords.x, _cardsMap.GetLength(0) - 1);
            int startY = Mathf.Clamp(originCardCoords.y, 0, originCardCoords.y);
            int endY = Mathf.Clamp(originCardCoords.y + _cardSpreadRange.y, originCardCoords.y, _cardsMap.GetLength(1) - 1);

            vacantCards.Add(originCard);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (_cardsMap[x, y] == null || _cardsMap[x, y].IsInited) continue;

                    vacantCards.Add(_cardsMap[x, y]);
                }
            }
        }

        // Executed for an additional set of cards if the cards count is less than 3.
        private void AddAbscentVacantCards(Vector2 originCardCoords, HashSet<Card> vacantCards)
        {
            while (vacantCards.Count < 3)
            {
                Card nearestCard = null;
                float nearestCardDistance = float.MaxValue;
                for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
                {
                    for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
                    {
                        Card card = _cardsMap[colunmI, cardI];
                        if (card == null || card.IsInited || vacantCards.Contains(card)) continue;

                        Vector2 cardCoords = new Vector2Int(colunmI, cardI);
                        float distance = Vector2.Distance(originCardCoords, cardCoords);

                        if (distance < nearestCardDistance)
                        {
                            nearestCardDistance = distance;
                            nearestCard = card;
                        }
                    }
                }

                if (nearestCard != null)
                {
                    vacantCards.Add(nearestCard);
                }    
            }
        }

        private List<Card> ShuffleCards(IEnumerable<Card> cards)
        {
            return cards.OrderBy(x => System.Guid.NewGuid()).ToList();
        }
    }
}
