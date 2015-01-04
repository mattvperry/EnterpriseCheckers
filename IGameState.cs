namespace EnterpriseCheckers
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for the checkers game state.
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// Finds all valid moves for a given team
        /// </summary>
        /// <param name="team">Team for moves</param>
        /// <returns>List of moves</returns>
        IEnumerable<Move> ValidMoves();

        /// <summary>
        /// Performs a move on the board
        /// </summary>
        /// <param name="move">Move to perform</param>
        void MakeMove(Move move);

        /// <summary>
        /// Decides if the game is over
        /// </summary>
        /// <returns>True if game is over</returns>
        bool GameOver();

        /// <summary>
        /// Decides the winning team
        /// </summary>
        /// <returns>Winning team</returns>
        Team WinningTeam();
    }
}
