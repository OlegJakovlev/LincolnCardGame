

namespace DeckOfCards.DeckStructure
{
    // Class responsible for Card object.
    class Card
    {
        public bool Selected { get; set; } = false;
        public string Name { get; private set; } = null;
        public string Suit { get; private set; }
        public int Value { get; private set; }

        public Card(string requstedSuit, int requestedValue)
        {
            Suit = requstedSuit;
            Value = requestedValue;
        }

        public Card(string requstedSuit, int requestedValue, string newName)
        {
            Suit = requstedSuit;
            Value = requestedValue;
            Name = newName;
        }
    }
}
