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

        private Card[,] _cardsMap;
        private CardLayoutSettings _layout;
        private int _maxColumnLength;

        public CardLayoutService(CardLayoutsSettings layouts, CardFactory cardFactory)
        {
            _layouts = layouts;
            _cardFactory = cardFactory;
        }

        public Card[,] SetUp(CardLayoutSettings layout)
        {
            _layout = layout;
            _maxColumnLength = _layout.GetMaxColumnLength();

            _cardsMap = new Card[_layout.ColumnCount, _maxColumnLength];

            CreateColumns();

            return _cardsMap;
        }

        private void CreateColumns()
        {
            for (int columnI = 0; columnI < _layout.ColumnCount; columnI++)
            {
                var column = _layout.CardColumns[columnI];
                Vector2 columnPosition = StartColumnsPosition + new Vector2(columnI * ColumnSpacing, 0);

                CreateColumn(column, columnPosition, columnI);
            }
        }

        private void CreateColumn(CardColumn column, Vector2 columnPosition, int columnI)
        {
            for (int cardI = 0; cardI < column.CardCount; cardI++)
            {
                Vector2 cardPosition = columnPosition + new Vector2(0, -_layouts.StepBetweenCards * cardI);
                _cardsMap[columnI, cardI] = _cardFactory.Create(cardPosition);
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
