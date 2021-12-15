using System;

namespace DeckOfCards.Exceptions
{
    class EmptyDeckException : Exception
    {
        public EmptyDeckException()
        {

        }

        public EmptyDeckException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.WriteLine("");
        }
    }
}
