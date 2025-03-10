using Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameplayServices
{
    public class FieldService
    {
        private Card[,] _cardsMap;
        private SlotBar _slotBar;

        public SlotBar SlotBar => _slotBar;

        public FieldService(Card[,] cardsMap, SlotBar slotBar)
        {
            _cardsMap = cardsMap;
            _slotBar = slotBar;
        }

        public IEnumerable<Card> Cards => _cardsMap.Cast<Card>();
        public int HorizontalLength => _cardsMap.GetLength(0);
        public int VerticalLength => _cardsMap.GetLength(1);

        public Card GetCard(int columnI, int rowI) => _cardsMap[columnI, rowI];
        public void SetCard(Card card, Vector2Int coordinates) => _cardsMap[coordinates.x, coordinates.y] = card;
        public bool IsCardExist(Card card) => card != null && !card.IsDestroyed;

        public bool HasAnyCard()
        {
            foreach (var card in _cardsMap)
            {
                if (IsCardExist(card))
                    return true;
            }

            return false;
        }
    }
}
