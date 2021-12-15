using System;

namespace DeckOfCards.Exceptions
{
    class WrongInputException : Exception
    {
        public WrongInputException()
        {
        }

        public WrongInputException(string message) : base(message)
        {
            Console.WriteLine(message);
            Console.WriteLine("");
        }
    }
}
