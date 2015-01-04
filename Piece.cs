namespace EnterpriseCheckers
{
    /// <summary>
    /// Class representing a checkers piece
    /// </summary>
    public class Piece
    {
        /// <summary>
        /// Team that owns the piece
        /// </summary>
        public Team Team { get; private set; }

        /* TODO: Implement kings
        /// <summary>
        /// Is this piece a king
        /// </summary>
        public bool King { get; set; }
        */

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        /// <param name="team">Team that owns the piece</param>
        public Piece(Team team)
        {
            this.Team = team;
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            switch(this.Team)
            {
                case Team.Red:
                    return "r";
                case Team.Black:
                    return "b";
                default:
                    return null;
            }
        }
    }
}
