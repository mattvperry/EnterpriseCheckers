namespace EnterpriseCheckers
{
    using System;
    using System.Linq;

    /// <summary>
    /// Enum representing the two teams
    /// </summary>
    public enum Team
    {
        Red,
        Black
    }

    /// <summary>
    /// Class representing a game of checkers
    /// </summary>
    public class Game : IGame
    {
        /// <summary>
        /// State instance associated with this game.
        /// </summary>
        private IGameState State { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="state">State class dependency</param>
        public Game(IGameState state)
        {
            this.State = state;
        }

        /// <summary>
        /// Run the game loop
        /// </summary>
        public void Run()
        {
            this.PrintWelcomeMessage();

            while(!this.State.GameOver())
            {
                // Print current state
                Console.WriteLine(this.State);

                // Get player move
                Console.WriteLine("Valid move list:");
                var moves = this.State.ValidMoves().ToList();
                foreach(var it in moves.Select((Move, i) => new { Move, Index = i + 1 }))
                {
                    Console.Write("{0}. ", it.Index);
                    Console.WriteLine(it.Move);
                }
                Console.WriteLine();
                int move = this.ReadMove(moves.Count());

                // Make move
                this.State.MakeMove(moves[move - 1]);

                Console.WriteLine();
            }

            Console.WriteLine("{0} team wins!", this.State.WinningTeam());
        }

        /// <summary>
        /// Reads from the console until the user inputs a number in the correct range
        /// </summary>
        /// <param name="max">Max input</param>
        /// <returns>Inputted number</returns>
        private int ReadMove(int max)
        {
            int move;
            while(true)
            {
                Console.Write("Choose move number: ");
                var input = Console.ReadLine();
                if(Int32.TryParse(input, out move))
                {
                    if(move > 0 && move <= max)
                    {
                        break;
                    }
                }

                Console.WriteLine("Please enter a number from the valid move list.");
            }
            return move;
        }

        /// <summary>
        /// Prints the game's welcome message
        /// </summary>
        private void PrintWelcomeMessage()
        {
            var welcomeMsg = @"
 _____ _               _
/  __ \ |             | |                
| /  \/ |__   ___  ___| | _____ _ __ ___ 
| |   | '_ \ / _ \/ __| |/ / _ \ '__/ __|
| \__/\ | | |  __/ (__|   <  __/ |  \__ \
 \____/_| |_|\___|\___|_|\_\___|_|  |___/";

            Console.WriteLine("{0}\n\n", welcomeMsg);
        }
    }
}
