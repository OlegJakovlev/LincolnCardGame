using DeckOfCards.UserFactory.Users;

namespace DeckOfCards.UserFactory
{
    abstract class Creator
    {
        public abstract IPlayer CreateUser(string type);
    }
}
