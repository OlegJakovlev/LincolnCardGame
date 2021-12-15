using DeckOfCards.DeckStructure;

namespace DeckOfCards.UserFactory.Users
{
    interface IPlayer
    {
        void Play(Card card);

        Card GetSelectedCard();

        void ResetHandScore();

        int GetHandCardCount();

        void GetNewHand(Deck deck);
    }
}
