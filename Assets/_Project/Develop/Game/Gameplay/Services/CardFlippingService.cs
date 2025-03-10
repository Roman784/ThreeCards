using Gameplay;
using R3;
using System.Collections.Generic;
using static GameplayServices.CardMatchingService;

namespace GameplayServices
{
    public class CardFlippingService
    {
        private FieldService _fieldService;

        public CardFlippingService(FieldService fieldService, 
                                   Observable<Card> onCardReadyToPlaced, Observable<List<RemovedCard>> onCardsRemoved)
        {
            _fieldService = fieldService;

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

            for (int colunmI = 0; colunmI < _fieldService.HorizontalLength; colunmI++)
            {
                for (int cardI = _fieldService.VerticalLength - 1; cardI >= 0; cardI--)
                {
                    Card card = _fieldService.GetCard(colunmI, cardI);

                    if (_fieldService.IsCardExist(card) && !_fieldService.SlotBar.ContainsCard(card))
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
            for (int colunmI = 0; colunmI < _fieldService.HorizontalLength; colunmI++)
            { 
                for (int cardI = 0; cardI < _fieldService.VerticalLength; cardI++)
                {
                    Card card = _fieldService.GetCard(colunmI, cardI);
                    if (card != currentCard || cardI < 1) continue;

                    Card nextCard = _fieldService.GetCard(colunmI, cardI - 1);
                    nextCard.Open();

                    return;
                }
            }
        }
    }
}
