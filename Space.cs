namespace EnterpriseCheckers
{
    /// <summary>
    /// Class representing a checkers board space
    /// </summary>
    public class Space
    {
        /// <summary>
        /// Enum representing all states of a checkers board space
        /// </summary>
        private enum SpaceState
        {
            Empty,
            Invalid,
            Occupied
        }

        /// <summary>
        /// Gets a new Invalid space
        /// </summary>
        public static Space Invalid
        {
            get
            {
                return new Space
                {
                    State = SpaceState.Invalid,
                };
            }
        }

        /// <summary>
        /// Gets a new Empty space
        /// </summary>
        public static Space Empty
        {
            get
            {
                return new Space
                {
                    State = SpaceState.Empty,
                };
            }
        }

        /// <summary>
        /// Gets a new space occupied by a red piece
        /// </summary>
        public static Space Red
        {
            get
            {
                return new Space
                {
                    State = SpaceState.Occupied,
                    Piece = new Piece(Team.Red)
                };
            }
        }

        /// <summary>
        /// Gets a new space occupied by a black piece
        /// </summary>
        public static Space Black
        {
            get
            {
                return new Space
                {
                    State = SpaceState.Occupied,
                    Piece = new Piece(Team.Black)
                };
            }
        }

        /// <summary>
        /// Gets the current piece of the space
        /// </summary>
        public Piece Piece { get; private set; }

        /// <summary>
        /// Gets or sets the current state of the space
        /// </summary>
        private SpaceState State { get; set; }

        /// <summary>
        /// Is this space occupied?
        /// </summary>
        /// <returns>True if occupied</returns>
        public bool Occupied()
        {
            return this.State == SpaceState.Occupied;
        }

        /// <summary>
        /// Is this space valid?
        /// </summary>
        /// <returns>True if valid</returns>
        public bool Valid()
        {
            return this.State != SpaceState.Invalid;
        }

        /// <summary>
        /// Place a piece on this space
        /// </summary>
        /// <param name="piece">Piece to place</param>
        public void Place(Piece piece)
        {
            this.Piece = piece;
            this.State = SpaceState.Occupied;
        }

        /// <summary>
        /// Clear a space of its piece
        /// </summary>
        public void Clear()
        {
            this.Piece = null;
            this.State = SpaceState.Empty;
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            switch(this.State)
            {
                case SpaceState.Empty:
                    return " ";
                case SpaceState.Invalid:
                    return string.Empty;
                case SpaceState.Occupied:
                    return this.Piece.ToString();
                default:
                    return null;
            }
        }
    }
}
