using Gameplay;
using Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Zenject;

namespace GameplayServices
{
    public class CardMarkingService
    {
        private Card[,] _cardsMap;
        private Vector2Int _vacantCardsRange;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider)
        {
            _vacantCardsRange = settingsProvider.GameSettings.CardLayoutsSettings.VacantCardsRange;
        }

        public void Mark(Card[,] cardsMap)
        {
            _cardsMap = cardsMap;

            for (int cardI = 0; cardI < cardsMap.GetLength(1); cardI++)
            {
                for (int colunmI = 0; colunmI < cardsMap.GetLength(0); colunmI++)
                {
                    Card card = cardsMap[colunmI, cardI];
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
                AddAbscentVacantCards(vacantCards);
            }

            var shuffledVacantCards = ShuffleCards(vacantCards);
            InitCards(shuffledVacantCards);
        }

        private void InitCards(List<Card> cards)
        {
            Suits suit = CardMarkingMapper.GetRandomSuits();

            for (int i = 0; i < 3; i++)
            {
                Card card = cards[i];
                Ranks rank = CardMarkingMapper.GetRandomRank();

                card.Init(suit, rank);
            }
        }

        private void SetVacantCards(HashSet<Card> vacantCards, Card originCard, Vector2Int originCardCoords)
        {
            int startX = Mathf.Clamp(originCardCoords.x - _vacantCardsRange.x, 0, originCardCoords.x);
            int endX = Mathf.Clamp(originCardCoords.x + _vacantCardsRange.x, originCardCoords.x, _cardsMap.GetLength(0) - 1);
            int startY = Mathf.Clamp(originCardCoords.y, 0, originCardCoords.y);
            int endY = Mathf.Clamp(originCardCoords.y + _vacantCardsRange.y, originCardCoords.y, _cardsMap.GetLength(1) - 1);

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

        private void AddAbscentVacantCards(HashSet<Card> vacantCards)
        {
            for (int x = 0; x < _cardsMap.GetLength(0); x++)
            {
                for (int y = 0; y < _cardsMap.GetLength(1); y++)
                {
                    Card card = _cardsMap[x, y];
                    if (card == null || card.IsInited || vacantCards.Contains(card)) continue;

                    vacantCards.Add(card);
                    break;
                }
            }
        }

        private List<Card> ShuffleCards(IEnumerable<Card> cards)
        {
            return cards.OrderBy(x => System.Guid.NewGuid()).ToList();
        }
    }
}
