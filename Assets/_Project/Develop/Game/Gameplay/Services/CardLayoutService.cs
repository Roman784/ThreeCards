using Gameplay;
using Settings;
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

        public CardLayoutService(CardLayoutsSettings layouts, CardFactory cardFactory, CardPlacingService placingService)
        {
            _layouts = layouts;
            _cardFactory = cardFactory;
            _placingService = placingService;
        }

        public Card[,] SetUp(CardLayoutSettings layout)
        {
            _layout = layout;
            _maxColumnLength = _layout.GetMaxColumnLength();

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
            for (int cardI = 0; cardI < column.CardCount; cardI++)
            {
                Vector2Int coordinates = new Vector2Int(columnI, cardI);
                Vector2 cardPosition = GetCardPosition(coordinates);

                Card card = _cardFactory.Create(cardPosition, coordinates);
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
