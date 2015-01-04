namespace EnterpriseCheckers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class representing a checkers board.
    /// </summary>
    public class Board : IBoard
    {
        /// <summary>
        /// Dictionary mapping 2D points to checkers pieces
        /// </summary>
        public Space[] Spaces { get; private set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Board"/> class.
        /// </summary>
        public Board()
        {
            this.Spaces = new Space[46];
            this.Initialize();
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            // Current piece to print
            var piecePointer = 5;
            var sb = new StringBuilder();

            // Three horizonal lines
            var bar = "\u2550\u2550\u2550";
            // The repeated piece of the top bar
            var top = String.Concat(Enumerable.Repeat(bar + "\u2566", 7));
            // The repeated piece of the middle bars
            var middle = String.Concat(Enumerable.Repeat(bar + "\u256C", 7));
            // The repeated piece of the bottom bar
            var bottom = String.Concat(Enumerable.Repeat(bar + "\u2569", 7));

            // The column numbers
            sb.Append("     1   2   3   4   5   6   7   8\n");
            // Top bar
            sb.AppendFormat("   \u2554{0}{1}\u2557\n", top, bar);

            // There are 8 rows
            foreach (var i in Enumerable.Range(0, 8))
            {
                // The row number
                sb.AppendFormat("{0}  ", i + 1);
                // There are 8 columns
                foreach (var j in Enumerable.Range(0, 8))
                {
                    // A vertical bar
                    sb.Append("\u2551");
                    // This is a piece square if both coordinates are even or if both are odd
                    if (i % 2 == j % 2)
                    {
                        // Append space and move pointer
                        sb.AppendFormat(" {0} ", this.Spaces[piecePointer++]);
                        while (piecePointer % 9 == 0) ++piecePointer;
                    }
                    else
                    {
                        // Not a piece square, fuzzy block chars
                        sb.Append("\u2591\u2591\u2591");
                    }
                }
                // Ending vertical bar
                sb.Append("\u2551\n");

                // There are only 7 middle bars
                if (i < 7)
                {
                    sb.AppendFormat("   \u2560{0}{1}\u2563\n", middle, bar);
                }
            }

            // Bottom bar
            sb.AppendFormat("   \u255A{0}{1}\u255D\n", bottom, bar);

            return sb.ToString();
        }

        /// <summary>
        /// Initializes board
        /// </summary>
        private void Initialize()
        {
            foreach (var i in Enumerable.Range(0, this.Spaces.Length))
            {
                if (i % 9 == 0 || i < 5 || i > 40)
                {
                    this.Spaces[i] = Space.Invalid;
                }
                else if (i >= 5 && i <= 17)
                {
                    this.Spaces[i] = Space.Black;
                }
                else if (i >= 28 && i <= 40)
                {
                    this.Spaces[i] = Space.Red;
                }
                else
                {
                    this.Spaces[i] = Space.Empty;
                }
            }
        }
    }
}
