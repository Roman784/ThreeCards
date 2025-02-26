using Gameplay;
using System;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class CardFactory : PlaceholderFactory<CardView>
    {
        public new Card Create()
        {
            CardView view = base.Create();
            Card card = new Card(view);

            return card;
        }

        public Card Create(Vector2Int coordinates)
        {
            Card card = Create();
            card.SetCoordinates(coordinates);

            return card;
        }

        public Card Create(Vector2 position, Vector2Int coordinates)
        {
            Card card = Create(coordinates);
            card.SetPosition(position);

            return card;
        }
    }
}
