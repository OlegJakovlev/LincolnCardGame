using DeckOfCards.DeckStructure;
using DeckOfCards.Exceptions;
using DeckOfCards.Logs;
using System;

namespace DeckOfCards.UserFactory.Users
{
    class Human : IPlayer
    {
        private Hand _hand;
        public readonly int ID;

        public int Score { get; set; } = 0;

        public int ScoreOfSelectedCards { get; private set; } = 0;

        public Human(Hand newHand, int newID)
        {
            _hand = newHand;
            ID = newID;
        }

        public void Play(Card cardToPlay)
        {
            Logger.WriteData($"Human (ID:{ID}) plays {cardToPlay.Suit} of {cardToPlay.Value}");
            ScoreOfSelectedCards += cardToPlay.Value;
            _hand.RemoveCardFromHand(cardToPlay);
        }

        public void GetNewHand(Deck deck)
        {
            _hand = new Hand(deck);
        }

        public Card GetSelectedCard()
        {
            foreach (Card card in _hand.CardsAvailable)
            {
                if (card.Selected) return card;
            }
            return null;
        }

        public void ResetHandScore()
        {
            ScoreOfSelectedCards = 0;
        }

        public void PrintHand()
        {
            Console.WriteLine("Which card to play?");
            int i = 0;
            foreach (Card card in _hand.CardsAvailable)
            {
                Console.WriteLine((i + 1) + ". " +
                    (card.Selected == true ? "[X]" : "[ ]") + " " +
                    (card.Name != null ? card.Name : card.Value.ToString()) + " of " + card.Suit + " " +
                    "(" + card.Value + ")"
                );
                i++;
            }
            Console.WriteLine((i + 1) + ". " + "Continue");
        }

        public int GetHandCardCount()
        {
            return _hand.CardsAvailable.Count;
        }

        public bool OnlyOneCardSelected()
        {
            int counter = 0;
            foreach (Card card in _hand.CardsAvailable)
            {
                if (card.Selected) counter++;
            }

            if (counter == 1) return true;
            return false;
        }

        public void SelectCard(int userInput)
        {
            userInput--;

            if (_hand.CardsAvailable[userInput].Selected)
            {
                _hand.CardsAvailable[userInput].Selected = false;
            }
            else
            {
                if (!OnlyOneCardSelected())
                {
                    _hand.CardsAvailable[userInput].Selected = true;
                }
                else
                {
                    throw new NoCardsWereSelectedException("Any other card already selected!");
                }
            }
        }
    }
}
