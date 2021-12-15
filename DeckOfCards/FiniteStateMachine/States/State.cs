using DeckOfCards.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace DeckOfCards.FiniteStateMachine.States
{
    abstract class State
    {
        protected FSM CurrentFSM;

        // Regex for validation of user input.
        private Regex _validator = new Regex(@"^[1-9]\d*$");

        public State(FSM fsm)
        {
            CurrentFSM = fsm;
        }

        public abstract void Enter();

        protected int ValidateAndHandleInput(int maxValue)
        {
            string userInput = Console.ReadLine();
            Console.WriteLine("");

            // Validate input with regexp.
            if (!_validator.IsMatch(userInput))
            {
                throw new WrongInputException("USER ERROR: Passed wrong option!");
            }

            // If integer provided is too large - handle the exception.
            int result;
            try
            {
                result = Int32.Parse(userInput);
            }
            catch (OverflowException)
            {
                result = 0;
            }

            // Validate based on number of available options.
            if (result > 0 && result <= maxValue)
            {
                return result;
            }
            else
            {
                throw new WrongInputException("USER ERROR: Passed wrong option!");
            }
        }

        // Display options from provided list.
        protected void DisplayOptionList(string[] optionsToDisplay)
        {
            Console.WriteLine("Choose an option:");
            for (int optionID = 0; optionID < optionsToDisplay.Length; optionID++)
            {
                Console.WriteLine((optionID + 1) + ". " + optionsToDisplay[optionID]);
            }
            Console.WriteLine("");
        }
    }
}
