namespace EnterpriseCheckers
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a checkers board.
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Dictionary mapping 2D points to checkers pieces
        /// </summary>
        Space[] Spaces { get; }
    }
}
