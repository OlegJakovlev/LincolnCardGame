using DeckOfCards.DeckStructure;
using DeckOfCards.Logs;
using System;

namespace DeckOfCards.UserFactory.Users
{
    class Computer : IPlayer
    {
        private Hand _hand;
        public readonly int ID;
        private readonly Random _random = new Random();

        public int Score { get; set; } = 0;
        public int ScoreOfSelectedCards { get; private set; } = 0;

        public Computer(Hand newHand, int newID)
        {
            _hand = newHand;
            ID = newID;
        }

        public void Play(Card cardToPlay)
        {
            Logger.WriteData($"PC (ID:{ID}) plays {cardToPlay.Suit} of {cardToPlay.Value}");
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

        public int GetHandCardCount()
        {
            return _hand.CardsAvailable.Count;
        }

        public Card SelectCard()
        {
            int cardIndex = _random.Next(0, _hand.CardsAvailable.Count);
            return _hand.CardsAvailable[cardIndex];
        }
    }
}
