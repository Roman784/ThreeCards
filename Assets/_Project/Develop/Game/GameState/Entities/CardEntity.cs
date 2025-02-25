using System;

namespace GameState
{
    [Serializable]
    public class CardEntity : Entity
    {
        public int Rank;
        public int Suit;
        public int ColumnIndex;
        public int RowIndex;
    }
}
