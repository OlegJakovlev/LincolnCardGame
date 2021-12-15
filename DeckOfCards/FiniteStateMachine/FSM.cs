using DeckOfCards.FiniteStateMachine.States;
using DeckOfCards.Logs;
using System;
using System.Collections.Generic;

namespace DeckOfCards.FiniteStateMachine
{
    class FSM
    {
        public State CurrentState = null;

        public enum StateIDList
        {
            Menu,
            Play
        }

        private Dictionary<int, State> _possibleStates = new Dictionary<int, State>();

        public FSM()
        {
            InitializeAllTheStates();
            Logger.WriteData("FSM Initialized");
        }

        private void InitializeAllTheStates()
        {
            AddState((int)StateIDList.Menu, new Menu(this));
            AddState((int)StateIDList.Play, new Play(this));
        }

        private void AddState(int stateID, State state)
        {
            _possibleStates.Add(stateID, state);
        }

        public State GetState(int stateID)
        {
            try
            {
                return _possibleStates[stateID];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"State with ID {stateID} was not found!");
                return null;
            }
        }

        public void ChangeState(State newState)
        {
            if (newState != null)
            {
                // Switch to new state.
                CurrentState = newState;

                // Enter the new state.
                CurrentState.Enter();
            }
        }
    }
}
