using System;

namespace DeckOfCards.Exceptions
{
    class NoCardsWereSelectedException : Exception
    {
        public NoCardsWereSelectedException()
        {

        }

        public NoCardsWereSelectedException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.WriteLine("");
        }
    }
}
