using Gameplay;
using Settings;
using System.Collections;
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

        // This is to alternate the color of the suit.
        private bool _isRedSuitOrder;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider)
        {
            _vacantCardsRange = settingsProvider.GameSettings.CardLayoutsSettings.VacantCardsRange;
        }

        public CardMarkingService()
        {
            _isRedSuitOrder = Random.Range()
        }

        public IEnumerator Mark(Card[,] cardsMap)
        {
            _cardsMap = cardsMap;

            for (int cardI = 0; cardI < cardsMap.GetLength(1); cardI++)
            {
                for (int colunmI = 0; colunmI < cardsMap.GetLength(0); colunmI++)
                {
                    Card card = cardsMap[colunmI, cardI];
                    if (card == null || card.IsInited) continue;

                    Vector2Int cardCoords = new Vector2Int(colunmI, cardI);
                    yield return Coroutines.StartRoutine(InitThreeCards(card, cardCoords));
                }
            }
        }

        private IEnumerator InitThreeCards(Card originCard, Vector2Int originCardCoords)
        {
            HashSet<Card> vacantCards = new();
            SetVacantCards(vacantCards, originCard, originCardCoords);

            if (vacantCards.Count < 3)
            {
                AddAbscentVacantCards(vacantCards);
            }

            var shuffledVacantCards = ShuffleCards(vacantCards);
            yield return Coroutines.StartRoutine(InitCards(shuffledVacantCards));
        }

        private IEnumerator InitCards(List<Card> cards)
        {
            yield return new WaitForSeconds(1);

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
                for (int y = _cardsMap.GetLength(1) - 1; y >= 0; y--)
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
