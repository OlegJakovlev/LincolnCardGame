using DeckOfCards.DeckStructure;
using DeckOfCards.FiniteStateMachine;
using DeckOfCards.Logs;
using DeckOfCards.UserFactory;
using DeckOfCards.UserFactory.Users;
using System;

namespace DeckOfCards
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Print greetings to the user.
            PrintGreetings();

            // Create FSM.
            FSM fsm = new FSM();

            // Change state of FSM to initial.
            fsm.ChangeState(fsm.GetState((int)FSM.StateIDList.Menu));
        }

        private static void PrintGreetings()
        {
            Console.WriteLine("Welcome to Magic Card Game");
        }
    }
}
