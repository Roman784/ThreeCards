using Gameplay;
using Settings;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;
using static Settings.CardLayoutSettings;

namespace GameplayServices
{
    public class CardLayoutService
    {
        private CardLayoutsSettings _layouts;
        private CardFactory _cardFactory;
        private CardPlacingService _placingService;

        private Card[,] _cardsMap;
        private CardLayoutSettings _layout;
        private int _maxColumnLength;

        private List<Vector2Int> _bombsCoordinates = new();

        public CardLayoutService(CardLayoutsSettings layouts, CardFactory cardFactory, CardPlacingService placingService)
        {
            _layouts = layouts;
            _cardFactory = cardFactory;
            _placingService = placingService;
        }

        public Card[,] SetUp(CardLayoutSettings layout)
        {
            _layout = layout;

            _bombsCoordinates.Capacity = _layout.BombsCount;
            var columnCount = _layout.ColumnCount;
            var maxColumnLengthWithoutBombs = _layout.GetMaxColumnLength();

            for (int i = 0; i < _layout.BombsCount; i++)
            {
                var x = Random.Range(0, columnCount);
                var y = Random.Range(0, _layout.CardColumns[x].CardCount);

                _bombsCoordinates.Add(new Vector2Int(x, y));
            }

            var maxColumnLengthOffsets = new Dictionary<int, int>();
            foreach (var bombCoordinates in _bombsCoordinates)
            {
                if (!maxColumnLengthOffsets.ContainsKey(bombCoordinates.x))
                    maxColumnLengthOffsets[bombCoordinates.x] = 0;
                maxColumnLengthOffsets[bombCoordinates.x] += 1;
            }
            var maxColumnLengthOffset = maxColumnLengthOffsets.Values.Max();

            _maxColumnLength = maxColumnLengthWithoutBombs + maxColumnLengthOffset;

            _cardsMap = new Card[_layout.ColumnCount, _maxColumnLength];

            CreateColumns();

            return _cardsMap;
        }

        public Vector2 GetCardPosition(Vector2Int coordinates)
        {
            Vector2 columnPosition = StartColumnsPosition + new Vector2(coordinates.x * ColumnSpacing, 0);
            return columnPosition + new Vector2(0, -_layouts.StepBetweenCards * coordinates.y);
        }

        private void CreateColumns()
        {
            for (int columnI = 0; columnI < _layout.ColumnCount; columnI++)
            {
                var column = _layout.CardColumns[columnI];
                CreateColumn(column, columnI);
            }
        }

        private void CreateColumn(CardColumn column, int columnI)
        {
            var offsetDueToBombs = _bombsCoordinates.Count(c => c.x == columnI);

            for (int cardI = 0; cardI < column.CardCount + offsetDueToBombs; cardI++)
            {
                Vector2Int coordinates = new Vector2Int(columnI, cardI);
                bool isBomb = _bombsCoordinates.Contains(coordinates);
                Vector2 cardPosition = GetCardPosition(coordinates);

                Card card = _cardFactory.Create(cardPosition, coordinates, isBomb);
                card.SetPlacingService(_placingService);
                card.Disable();

                _cardsMap[columnI, cardI] = card;
            }
        }

        private Vector2 StartColumnsPosition
        {
            get
            {
                float totalWidth = (_layout.ColumnCount - 1) * ColumnSpacing;
                return _layouts.ColumnsOffset + new Vector2(totalWidth, _maxColumnLength * -_layouts.StepBetweenCards) / -2f;
            }
        }

        private float ColumnSpacing => _layouts.CalculateColumnSpacing(_layout.ColumnCount);
    }
}
