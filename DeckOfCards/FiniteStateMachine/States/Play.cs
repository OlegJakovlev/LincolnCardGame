using DeckOfCards.DeckStructure;
using DeckOfCards.Exceptions;
using DeckOfCards.Logs;
using DeckOfCards.UserFactory;
using DeckOfCards.UserFactory.Users;
using System;

namespace DeckOfCards.FiniteStateMachine.States
{
    class Play : State
    {
        private Deck _customDeck;
        private Creator _userFactory;

        private Random _random = new Random(Guid.NewGuid().GetHashCode());

        // Options to choose from.
        private string[] _options = {
            "Select cards from hand to play",
            "End the game"
        };

        // State status.
        private bool _isActive = true;

        // Players objects.
        private Human _human;
        private Computer _PC;

        // Determines if player should move first.
        private bool _playerMove;

        // How many hands are at stake.
        private int _pointsAtStake = 1;

        public Play(FSM fsm) : base(fsm)
        {
            // Create user factory.
            _userFactory = new ConcreateCreator();
        }

        public override void Enter()
        {
            Logger.WriteData("User entered the play state");

            // Set state active
            _isActive = true;

            // Generate new deck for new game.
            _customDeck = new Deck();
            ((ConcreateCreator) _userFactory).CurrentDeck = _customDeck;

            // Create new users for new game.
            _human = _userFactory.CreateUser("Human") as Human;
            Logger.WriteData($"Human (ID:{ _human.ID }) user created");

            _PC = _userFactory.CreateUser("PC") as Computer;
            Logger.WriteData($"PC (ID:{ _PC.ID }) user created");

            // Decide who will move first in the game.
            _playerMove = Convert.ToBoolean(_random.Next(0, 2));

            // Print options and get valid input from user.
            ChooseAnOption();
        }

        private void ChooseAnOption()
        {
            while (_isActive)
            {
                int userChoise = -1;

                // Get good input.
                while (userChoise == -1)
                {
                    try
                    {
                        PrintScore();

                        // Display options to user.
                        DisplayOptionList(_options);
                        userChoise = ValidateAndHandleInput(_options.Length);
                    }
                    catch (WrongInputException)
                    {
                        // Handle the exception.
                    }
                }

                // Activate chosed option.
                try
                {
                    switch (userChoise)
                    {
                        case 1:
                            StartGame();
                            CheckWinner();
                            break;

                        case 2:
                            //CheckGameWinner();
                            _isActive = false;
                            CurrentFSM.ChangeState(CurrentFSM.GetState((int)FSM.StateIDList.Menu));
                            break;
                    }
                }
                catch (WrongInputException)
                {
                    // Handle the exception.
                }
            }
        }

        private void PrintScore()
        {
            Console.WriteLine("Total Score:");
            Console.WriteLine($"You - { _human.Score }\nPC - { _PC.Score }\n");
            Logger.WriteData($"Total Score: Human - { _human.Score }\tPC - { _PC.Score }");
        }

        private void PrintPlayedCardScore()
        {
            Console.WriteLine("Played cards value:");
            Logger.WriteData($"Human - { _human.ScoreOfSelectedCards }\tPC - { _PC.ScoreOfSelectedCards }");
            Console.WriteLine($"You - { _human.ScoreOfSelectedCards }\nPC - { _PC.ScoreOfSelectedCards }");
            Console.WriteLine("");
        }

        private void StartGame()
        {
            Logger.WriteData("User started the game");

            // Check if everyone got enought cards in hand.
            if (_human.GetHandCardCount() <= 0 || _PC.GetHandCardCount() <= 0)
            {
                Logger.WriteData("No cards in Hand! Getting cards from deck");
                Console.WriteLine("No cards in Hand! Getting cards from deck!\n");
                GiveCardsToPlayers();
            }

            // Start the game.
            if (_playerMove == true)
            {
                PlayerMove();
                _PC.Play(_PC.SelectCard());

                PrintPlayedCardScore();

                PlayerMove();
                _PC.Play(_PC.SelectCard());
            }
            else
            {
                _PC.Play(_PC.SelectCard());
                PlayerMove();

                PrintPlayedCardScore();

                _PC.Play(_PC.SelectCard());
                PlayerMove();
            }
        }

        private void GiveCardsToPlayers()
        {
            // If current deck has less than 20 cards - create new.
            if (_customDeck.CardsInCurrentDeck() < 20)
            {
                Logger.WriteData($"Current deck has insufficient cards. Creating new one!");
                Console.WriteLine("Current deck has insufficient cards. Creating new one!\n");
                _customDeck = new Deck();
            }

            _human.GetNewHand(_customDeck);
            _PC.GetNewHand(_customDeck);
        }

        private void CheckWinner()
        {
            PrintPlayedCardScore();

            if (_human.ScoreOfSelectedCards > _PC.ScoreOfSelectedCards || _PC.ScoreOfSelectedCards > _human.ScoreOfSelectedCards)
            {
                if (_human.ScoreOfSelectedCards > _PC.ScoreOfSelectedCards)
                {
                    _human.Score += _pointsAtStake;

                    Logger.WriteData($"Player won { _pointsAtStake } point(-s)!");
                    Console.WriteLine($"Player won { _pointsAtStake } point(-s)!");

                    // Who wins make first move on next turn.
                    _playerMove = true;
                }
                else
                {
                    _PC.Score += _pointsAtStake;

                    Logger.WriteData($"PC won { _pointsAtStake } point(-s)!");
                    Console.WriteLine($"PC won { _pointsAtStake } point(-s)!");

                    // Who wins make first move on next turn.
                    _playerMove = false;
                }

                ResetPointsAtStake();
            }
            else
            {
                Logger.WriteData($"DRAW. At Stake: { _pointsAtStake } point(-s)!");
                Console.WriteLine($"It is DRAW! At Stake is: { _pointsAtStake } point(-s)!");

                // Increment points to give for winning the next hand.
                _pointsAtStake++;

                // If player made move first, now it is PC turn.
                _playerMove = !_playerMove;
            }

            // In case last cards were played, but need to pick the game winner.
            if (_human.GetHandCardCount() == 0 && _PC.GetHandCardCount() == 0)
            {
                CheckGameWinner();
                _isActive = false;
                CurrentFSM.ChangeState(CurrentFSM.GetState((int)FSM.StateIDList.Menu));
                return;
            }

            _human.ResetHandScore();
            _PC.ResetHandScore();
        }

        private void CheckGameWinner()
        {
            if (_human.Score == _PC.Score)
            {
                Logger.WriteData("Hand score draw! Random Last Card method!");
                Console.WriteLine("There is a draw by hand score! All depends on Last Random Card!");

                bool winnerFound = false;
                Card humanCard;
                Card PCCard;

                while (!winnerFound)
                {
                    // In case all cards are being played
                    if (_customDeck.IsEmpty())
                    {
                        Console.WriteLine("Deck is empty, getting new one");
                        Logger.WriteData("Deck is empty, getting new one");
                        _customDeck = new Deck();
                    }

                    humanCard = _customDeck.Deal();
                    PCCard = _customDeck.Deal();

                    Console.WriteLine($"Final Random Human Card: {humanCard.Value} \n" +
                        $"Final Random PC Card: {PCCard.Value} \n");

                    Logger.WriteData($"Final Random Human Card: {humanCard.Value} - Final Random PC Card: {PCCard.Value} ");

                    if (humanCard.Value < PCCard.Value)
                    {
                        winnerFound = true;
                        _PC.Score++;
                    }
                    else if (humanCard.Value > PCCard.Value)
                    {
                        winnerFound = true;
                        _human.Score++;
                    }
                    else
                    {
                        Console.WriteLine("Random picked cards were equal! Picking another random cards...");
                        Logger.WriteData("Random picked cards were equal! Picking another random cards...");
                    }
                }
            }

            ResetPointsAtStake();
            PrintScore();

            Console.WriteLine($"Overall game winner is { (_human.Score > _PC.Score ? "YOU" : "PC") }");
            Logger.WriteData($"Overall game winner is { (_human.Score > _PC.Score ? "YOU" : "PC") }");
        }

        private void ResetPointsAtStake()
        {
            _pointsAtStake = 1;
        }

        private void PlayerMove()
        {
            int userInput = -1;

            while (userInput != _human.GetHandCardCount()+1)
            {
                try
                {
                    _human.PrintHand();

                    // All cards + continue option.
                    userInput = ValidateAndHandleInput(_human.GetHandCardCount()+1);

                    // Continue option.
                    if (userInput == _human.GetHandCardCount()+1)
                    {
                        if (_human.OnlyOneCardSelected())
                        {
                            // Play the card.
                            _human.Play(_human.GetSelectedCard());
                            break;
                        }
                        else
                        {
                            throw new NoCardsWereSelectedException("No Cards / Too Much Cards Were Selected!");
                        }
                    }
                    // Select card.
                    else
                    {
                        _human.SelectCard(userInput);
                        userInput = -1;
                    }
                }
                catch (WrongInputException)
                {
                    userInput = -1;
                }
                catch (NoCardsWereSelectedException)
                {
                    userInput = -1;
                }
            }
        }
    }
}
