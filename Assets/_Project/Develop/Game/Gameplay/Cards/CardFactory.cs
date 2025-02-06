using Gameplay;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class CardFactory : PlaceholderFactory<CardView>
    {
        public Card Create(Vector2 position)
        {
            CardView view =  base.Create();
            Card card = new Card(view);

            view.transform.position = position;

            return card;
        }
    }
}
