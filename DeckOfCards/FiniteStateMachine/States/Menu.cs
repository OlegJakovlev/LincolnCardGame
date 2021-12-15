using DeckOfCards.Exceptions;
using DeckOfCards.Logs;
using System;

namespace DeckOfCards.FiniteStateMachine.States
{
    class Menu : State
    {
        private string[] _options = {
            "Start new game",
            "Rules of the game",
            "Exit"
        };

        private bool _isActive = true;

        public Menu(FSM fsm) : base(fsm)
        {
        }

        public override void Enter()
        {
            Logger.WriteData("User entered the menu");
            _isActive = true;

            while (_isActive)
            {
                Console.WriteLine("");

                int inputValue = -1;

                // Get good input.
                while (inputValue == -1)
                {
                    try
                    {
                        DisplayOptionList(_options);
                        inputValue = ValidateAndHandleInput(_options.Length);
                    }
                    catch (WrongInputException)
                    {
                        // Handle the exception.
                    }
                }

                try
                {
                    switch (inputValue)
                    {
                        // Start the game.
                        case 1:
                            _isActive = false;
                            CurrentFSM.ChangeState(CurrentFSM.GetState((int)FSM.StateIDList.Play));
                            break;

                        // Print rules of the game
                        case 2:
                            Console.WriteLine("Lincoln card game rules:");
                            Console.WriteLine("1. Each from 2 players gets 10 cards from deck. Each card have it value from 2 up to 14.");
                            Console.WriteLine("2. Players takes moves by playing 1 card each(2 in total)");
                            Console.WriteLine("3. Player with highes sum of card values wins the round and makes move first on the next round.");
                            Console.WriteLine("4. If totals are the same, continue to the next hand. Winner of that gets both rounds.");
                            Console.WriteLine("5. If the number of hand wins are the same, draw a random card from the remaining cards - highest wins.");
                            Console.WriteLine("6. If the final hands are the same value, draw a random card from the remaining cards - highest wins the hand.");
                            Console.WriteLine("7. Player with highest number of hand wins, wins the game");
                            break;

                        // Exit.
                        case 3:
                            _isActive = false;
                            Logger.WriteData("User exists the application");
                            Console.WriteLine("Thanks for using our Software!");
                            break;

                        default:
                            throw new WrongInputException("USER ERROR: Passed wrong option");
                    }
                }
                catch (WrongInputException)
                {
                    // Handle exception.
                }
            }
        }
    }
}
