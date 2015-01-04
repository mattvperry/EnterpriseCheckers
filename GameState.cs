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
            foreach(var it in this.Board.Spaces
                .Select((Space, Index) => new { Space, Index })
                .Where(it => it.Space.Occupied() && it.Space.Piece.Team == this.CurrentTurn))
            {
                moves.AddRange(this.FindMoves(this.CurrentTurn, new Move(it.Index, it.Index)));
            }

            // Forced jumps
            var jumpMoves = moves.Where(m => m.Jumps.Any());
            return jumpMoves.Any() ? jumpMoves : moves;
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

            if (move.KingMe)
            {
                toSpace.Piece.King = true;
            }

            move.Jumps.ForEach(j => this.Board.Spaces[j].Clear());

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
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            var turnMsg = string.Format("It is currently the {0} team's turn.", this.CurrentTurn);
            var winMsg = string.Format("{0} team wins!", this.WinningTeam());

            return string.Format("{0}\n\n{1}", this.GameOver() ? winMsg : turnMsg, this.Board);
        }

        /// <summary>
        /// Decides the winning team
        /// </summary>
        /// <returns>Winning team</returns>
        private Team WinningTeam()
        {
            return this.PieceCount(Team.Red) > this.PieceCount(Team.Black) ? Team.Red : Team.Black;
        }

        /// <summary>
        /// Find all moves starting with a seed move
        /// </summary>
        /// <param name="team">Team to move</param>
        /// <param name="move">Seed move</param>
        /// <returns>List of all possible moves</returns>
        private IEnumerable<Move> FindMoves(Team team, Move move)
        {
            var moves = new List<Move>();
            var multiplier = this.CurrentTurn == Team.Black ? 1 : -1;

            // Try both forward directions
            moves.AddRange(this.FindMoves(this.CurrentTurn, new Move(move), multiplier * 4));
            moves.AddRange(this.FindMoves(this.CurrentTurn, new Move(move), multiplier * 5));

            // Piece is already a king or this move will make it one
            if(this.Board.Spaces[move.From].Piece.King || move.KingMe)
            {
                // Try both backward directions
                moves.AddRange(this.FindMoves(this.CurrentTurn, new Move(move), -1 * multiplier * 4));
                moves.AddRange(this.FindMoves(this.CurrentTurn, new Move(move), -1 * multiplier * 5));
            }

            // There are no moves to be made and the seed move is not a "pass"
            if(!moves.Any() && move.To != move.From)
            {
                moves.Add(move);
            }

            return moves;
        }

        /// <summary>
        /// Find all moves from one direction
        /// </summary>
        /// <param name="team">Team to move</param>
        /// <param name="move">Starting move</param>
        /// <param name="direction">Direction to move</param>
        /// <returns>List of moves</returns>
        private IEnumerable<Move> FindMoves(Team team, Move move, int direction)
        {
            var moves = new List<Move>();
            var possibleSpace = this.Board.Spaces[move.To + direction];
            // Only consider direction if it is valid and we haven't already jumped it
            if(possibleSpace.Valid() && !move.Jumps.Contains(move.To + direction))
            {
                // Simple move possible. If this move already has jumps we can't make simple moves
                if (!possibleSpace.Occupied() && !move.Jumps.Any())
                {
                    move.To += direction;
                    if(KingSpace(team, move.To))
                    {
                        move.KingMe = true;
                    }
                    moves.Add(move);
                }
                else if (possibleSpace.Occupied() && possibleSpace.Piece.Team != team)
                {
                    possibleSpace = this.Board.Spaces[move.To + 2 * direction];
                    // At least single jump possible
                    if (possibleSpace.Valid() && !possibleSpace.Occupied())
                    {
                        move.Jumps.Add(move.To + direction);
                        move.To += 2 * direction;
                        if(KingSpace(team, move.To))
                        {
                            move.KingMe = true;
                        }
                        // Recurse and look for more jumps
                        moves.AddRange(this.FindMoves(team, move));
                    }
                }
            }
            return moves;
        }

        /// <summary>
        /// Is the coordinate a king space for this team?
        /// </summary>
        /// <param name="team">Team to check</param>
        /// <param name="coord">Coordinate to check</param>
        /// <returns>True if a king space</returns>
        private bool KingSpace(Team team, int coord)
        {
            if(team == Team.Red && coord >= 5 && coord <= 8)
            {
                return true;
            }

            if(team == Team.Black && coord >= 37 && coord <= 40)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Count remaining pieces for given team.
        /// </summary>
        /// <param name="team">Team to check</param>
        /// <returns>Number of remaining pieces</returns>
        private int PieceCount(Team team)
        {
            return this.Board.Spaces.Where(s => s.Occupied() && s.Piece.Team == team).Count();
        }
    }
}
