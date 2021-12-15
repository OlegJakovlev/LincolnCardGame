using System;
using System.Collections.Generic;

namespace DeckOfCards.DeckStructure
{
    class Hand
    {
        public List<Card> CardsAvailable = new List<Card>();

        public Hand(Deck currentDeck)
        {
            try
            {
                CardsAvailable.AddRange(currentDeck.Deal(10));

                if (CardsAvailable.Count < 10)
                {
                    throw new Exception("Not enought cards were dealt!");
                }
            }
            catch (Exception)
            {
                // Handle the exception.
            }
        }

        public void RemoveCardFromHand(Card cardToDelete)
        {
            CardsAvailable.Remove(cardToDelete);
        }
    }
}
