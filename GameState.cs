namespace EnterpriseCheckers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class representing the current state of a game.
    /// </summary>
    public class GameState : IGameState
    {
        /// <summary>
        /// Board instance associated with this gamestate.
        /// </summary>
        private IBoard Board { get; set; }

        /// <summary>
        /// Gets or sets the current turn
        /// </summary>
        private Team CurrentTurn { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="board">Board class dependency</param>
        public GameState(IBoard board)
        {
            this.Board = board;
            this.CurrentTurn = Team.Red;
        }

        /// <summary>
        /// Finds all valid moves for the current team
        /// </summary>
        /// <returns>List of moves</returns>
        public IEnumerable<Move> ValidMoves()
        {
            var moves = new List<Move>();
            var multiplier = this.CurrentTurn == Team.Black ? 1 : -1;

            foreach(var it in this.Board.Spaces.Select((Space, Index) => new { Space, Index }))
            {
                if(it.Space.Occupied() && it.Space.Piece.Team == this.CurrentTurn)
                {
                    // Try one direction
                    moves.AddRange(this.FindMoves(this.CurrentTurn, it.Index, multiplier * 4));

                    // Try the other
                    moves.AddRange(this.FindMoves(this.CurrentTurn, it.Index, multiplier * 5));
                }
            }

            // Forced jumps
            var jumpMoves = moves.Where(m => m.Jumps.Any());
            if (jumpMoves.Any())
            {
                return jumpMoves;
            }

            return moves;
        }

        /// <summary>
        /// Performs a move on the board
        /// </summary>
        /// <param name="move">Move to perform</param>
        public void MakeMove(Move move)
        {
            var fromSpace = this.Board.Spaces[move.From];
            var toSpace = this.Board.Spaces[move.To];

            toSpace.Place(fromSpace.Piece);
            fromSpace.Clear();

            foreach(var jumped in move.Jumps)
            {
                this.Board.Spaces[jumped].Clear();
            }

            // Toggle current turn
            this.CurrentTurn = (Team)((((int)this.CurrentTurn) + 1) % 2);
        }

        /// <summary>
        /// Decides if the game is over
        /// </summary>
        /// <returns>True if game is over</returns>
        public bool GameOver()
        {
            return new int[] { this.PieceCount(Team.Red), this.PieceCount(Team.Black) }.Any(c => c == 0);
        }

        /// <summary>
        /// Decides the winning team
        /// </summary>
        /// <returns>Winning team</returns>
        public Team WinningTeam()
        {
            return this.PieceCount(Team.Red) > this.PieceCount(Team.Black) ? Team.Red : Team.Black;
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            var turnMsg = string.Format("It is currently the {0} team's turn.", this.CurrentTurn);

            return string.Format("{0}\n\n{1}", turnMsg, this.Board);
        }

        /// <summary>
        /// Find all moves from one direction
        /// </summary>
        /// <param name="team">Team to move</param>
        /// <param name="start">Starting piece</param>
        /// <param name="direction">Direction to move</param>
        /// <returns>List of moves</returns>
        private IEnumerable<Move> FindMoves(Team team, int start, int direction)
        {
            var moves = new List<Move>();
            var destination = start + direction;
            var possibleSpace = this.Board.Spaces[destination];
            if(possibleSpace.Valid())
            {
                // Simple move possible
                if (!possibleSpace.Occupied())
                {
                    moves.Add(new Move(start, destination));
                }
                else if (possibleSpace.Piece.Team != team)
                {
                    destination += direction;
                    possibleSpace = this.Board.Spaces[destination];
                    // At least single jump possible
                    if (possibleSpace.Valid() && !possibleSpace.Occupied())
                    {
                        var move = new Move(start, destination);
                        move.Jumps.Add(start + direction);
                        moves.Add(move);
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Count remaining pieces for given team.
        /// </summary>
        /// <param name="team">Team to check</param>
        /// <returns>Number of remaining pieces</returns>
        private int PieceCount(Team team)
        {
            return this.Board.Spaces.Where(p => p.Occupied() && p.Piece.Team == team).Count();
        }
    }
}
