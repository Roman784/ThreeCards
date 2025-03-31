using Gameplay;
using System;
using UI;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class CardFactory : PlaceholderFactory<CardView>
    {
        private GameplayTools _gameplayTools;

        [Inject]
        private void Construct(GameplayTools gameplayTools)
        {
            _gameplayTools = gameplayTools;
        }

        public new Card Create()
        {
            CardView view = base.Create();
            Card card = new Card(view);

            card.SetGameplayTools(_gameplayTools);

            return card;
        }

        public Card Create(bool isBombs = false)
        {
            Card card = Create();
            card.SetIsBomb(isBombs);

            return card;
        }

        public Card Create(Vector2Int coordinates, bool isBombs = false)
        {
            Card card = Create(isBombs);
            card.SetCoordinates(coordinates);

            return card;
        }

        public Card Create(Vector2 position, Vector2Int coordinates, bool isBombs = false)
        {
            Card card = Create(coordinates, isBombs);
            card.SetPosition(position);

            return card;
        }
    }
}
