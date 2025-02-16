using Gameplay;
using System.Collections;
using UnityEngine;
using Utils;

namespace GameplayServices
{
    public class FieldAnimationService
    {
        private Card[,] _cardsMap;
        private CardFlippingService _cardFlippingService;

        public FieldAnimationService(Card[,] cardsMap, CardFlippingService cardFlippingService)
        {
            _cardsMap = cardsMap;
            _cardFlippingService = cardFlippingService;
        }

        public void LayOutCards()
        {
            Coroutines.StartRoutine(LayOutCardsRoutine());
        }

        private IEnumerator LayOutCardsRoutine()
        {
            for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
            {
                for (int colunmI = 0; colunmI < _cardsMap.GetLength(0); colunmI++)
                {
                    Card card = _cardsMap[colunmI, cardI];
                    if (card == null) continue;

                    card.PutDown();

                    yield return new WaitForSeconds(0.035f);
                }
            }

            yield return new WaitForSeconds(0.4f);

            _cardFlippingService.OpenFirstCards();
        }
    }
}
