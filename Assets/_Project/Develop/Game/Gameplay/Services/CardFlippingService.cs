using Gameplay;
using R3;
using System.Collections.Generic;
using static GameplayServices.CardMatchingService;

namespace GameplayServices
{
    public class CardFlippingService
    {
        private Card[,] _cardsMap;
        private SlotBar _slotBar;

        public CardFlippingService(Card[,] cardsMap, SlotBar slotBar, 
                                   Observable<Card> onCardReadyToPlaced, Observable<List<RemovedCard>> onCardsRemoved)
        {
            _cardsMap = cardsMap;
            _slotBar = slotBar;

            onCardReadyToPlaced.Subscribe(card => OpenNextCard(card));
            onCardsRemoved.Subscribe(_ => OpenFirstCards());
        }

        public Observable<Unit> OpenFirstCards()
        {
            Observable<Unit> cardsOpened = null;

            foreach (var card in GetFirstCards())
            {
                if (card.IsClosed)
                    cardsOpened = card.Open();
            }

            return cardsOpened;
        }

        public Observable<Unit> CloseFirstCards()
        {
            Observable<Unit> cardsClosed = null;

            foreach (var card in GetFirstCards())
            {
                if (!card.IsClosed)
                    cardsClosed = card.Close();
            }

            return cardsClosed;
        }

        private List<Card> GetFirstCards()
        {
            var cards = new List<Card>();

            for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
            {
                for (int cardI = _cardsMap.GetLength(1) - 1; cardI >= 0; cardI--)
                {
                    Card card = _cardsMap[colunmI, cardI];

                    if (card != null && !card.IsDestroyed && !_slotBar.ContainsCard(card))
                    {
                        cards.Add(card);
                        break;
                    }
                }
            }

            return cards;
        }

        private void OpenNextCard(Card currentCard)
        {
            for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
            { 
                for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
                {
                    Card card = _cardsMap[colunmI, cardI];
                    if (card != currentCard || cardI < 1) continue;

                    Card nextCard = _cardsMap[colunmI, cardI - 1];
                    nextCard.Open();
                    return;
                }
            }
        }
    }
}
