using System;
using System.Collections.Generic;
using System.Linq;
using DeckOfCards.Exceptions;

namespace DeckOfCards.DeckStructure
{
    // Class responsible for Deck object.
    class Deck
    {
        // Handles all cards available in deck.
        private List<Card> _cards = new List<Card>();

        // Handles cards while dealing multiple cards.
        private List<Card> _dealingCards = new List<Card>();

        // Used for temporary storing shuffled deck.
        private List<Card> _shuffleCards = new List<Card>();

        // Random object for dealing the card.
        private readonly Random _random = new Random();

        private List<int> _cardPossibleValues = new List<int>(Enumerable.Range(2, 13));
        private enum _possibleSuits { Diamonds, Clubs, Hearts, Spades };

        public Deck()
        {
            // Generate the cards.
            foreach (int value in _cardPossibleValues)
            {
                foreach (string suit in Enum.GetNames(typeof(_possibleSuits)))
                {
                    if (value == 11)
                    {
                        _cards.Add(new Card(suit, value,"Jack"));
                        continue;
                    }
                    if (value == 12)
                    {
                        _cards.Add(new Card(suit, value, "Queen"));
                        continue;
                    }
                    if (value == 13)
                    {
                        _cards.Add(new Card(suit, value, "King"));
                        continue;
                    }
                    if (value == 14)
                    {
                        _cards.Add(new Card(suit, value, "Ace"));
                        continue;
                    }

                    _cards.Add(new Card(suit, value));
                }
            }

            Shuffle();
        }

        public bool IsEmpty()
        {
            return (_cards.Count == 0) ? throw new EmptyDeckException("Deck is empty! Please, create a new one!") : false;
        }

        private void Shuffle()
        {
            try
            {
                // If is empty or only 1 card - no need to shuffle.
                if (IsEmpty() || _shuffleCards.Count == 1) return;

                // Clear shuffle cards list from previous shuffle.
                _shuffleCards.Clear();

                // Save how many cards we need to have in result.
                int cardsToHave = _cards.Count;

                // As Deal method returns random card, we can use it for shuffle.
                while (_shuffleCards.Count != cardsToHave)
                {
                    _shuffleCards.Add(Deal());
                }

                // Clear cards list.
                _cards.Clear();

                // Copy new shuffled deck in cards.
                _cards.AddRange(_shuffleCards);
            }
            catch (EmptyDeckException)
            {
                // Handle the exception.
            }
        }

        public Card Deal()
        {
            try
            {
                // Check if deck is empty.
                IsEmpty();

                // Get random card from deck
                int randomIndex = _random.Next(0, _cards.Count);
                Card randomCard = _cards[randomIndex];
                _cards.RemoveAt(randomIndex);

                // Return the card
                return randomCard;
            }
            catch (EmptyDeckException)
            {
                // Handle the exception.
                return null;
            }
        }

        public List<Card> Deal(int cardsAmountToDeal)
        {
            // Clean list from previous entry.
            _dealingCards.Clear();

            for (int i = 0; i < cardsAmountToDeal; i++) 
            {
                // Get a card from deck.
                Card card = Deal();

                // If deck is not empty yet - add card to list.
                if (card != null)
                {
                    _dealingCards.Add(card);
                }
                else
                {
                    break;
                }
            }
            
            return _dealingCards;
        }

        // Used only for testing purposes.
        public void PrintDeck()
        {
            try
            {
                // Check if deck is empty.
                IsEmpty();

                Console.WriteLine("In current deck available cards are: ");
                foreach (Card card in _cards)
                {
                    if (card.Name != null) {
                        Console.WriteLine(card.Name + " of " + card.Suit);
                    }
                    else {
                        Console.WriteLine(card.Value + " of " + card.Suit);
                    }
                }
            }
            catch (EmptyDeckException)
            {
                // Handle the exception.
            } 
        }

        public int CardsInCurrentDeck()
        {
            return _cards.Count;
        }
    }
}
