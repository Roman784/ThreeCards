using Gameplay;
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

        public Card Create(Vector2 position)
        {
            Card card = Create();
            card.View.transform.position = position;

            return card;
        }
    }
}
