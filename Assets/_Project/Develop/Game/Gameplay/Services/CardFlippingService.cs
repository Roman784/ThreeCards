using Gameplay;
using R3;

namespace GameplayServices
{
    public class CardFlippingService
    {
        private Card[,] _cardsMap;

        public CardFlippingService(Card[,] cardsMap, CardMatchingService cardMatchingService)
        {
            _cardsMap = cardsMap;

            cardMatchingService.OnCardPlaced.Subscribe(card => OpenNextCard(card));
        }

        public void OpenFirstCards()
        {
            for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
            {
                for (int cardI = _cardsMap.GetLength(1) - 1; cardI >= 0; cardI--)
                {
                    Card card = _cardsMap[colunmI, cardI];
                    if (card == null) continue;

                    card.Open();
                    break;
                }
            }
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
