using DeckOfCards.DeckStructure;
using DeckOfCards.UserFactory.Users;
using System;

namespace DeckOfCards.UserFactory
{
    class ConcreateCreator : Creator
    {
        private static int s_ID = 0;
        public Deck CurrentDeck { private get; set; }

        public ConcreateCreator() { }

        public override IPlayer CreateUser(string type)
        {
            switch (type)
            {
                case "PC":
                    s_ID++;
                    return new Computer(new Hand(CurrentDeck), s_ID);

                case "Human":
                    s_ID++;
                    return new Human(new Hand(CurrentDeck), s_ID);

                default: throw new ArgumentException("Invalid Type", type);
            }
        }
    }
}
