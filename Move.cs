namespace EnterpriseCheckers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Class representation of a checkers game move
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Starting coordinate
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Ending coordinate
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// List of jumped pieces
        /// </summary>
        public List<int> Jumps { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="from">Starting coordinate</param>
        /// <param name="to">Ending coordinate</param>
        public Move(int from, int to)
        {
            this.From = from;
            this.To = to;
            this.Jumps = new List<int>();
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            var fromPt = this.CoordinateToPoint(this.From);
            var toPt = this.CoordinateToPoint(this.To);

            return string.Format("({0}, {1}) to ({2}, {3})", fromPt.X, fromPt.Y, toPt.X, toPt.Y);
        }

        /// <summary>
        /// Crazy math to turn our coordinate system into a 2D point
        /// </summary>
        /// <param name="coord">Input coordinate</param>
        /// <returns>Point in 2D space</returns>
        private Point CoordinateToPoint(int coord)
        {
            // This was entirely worked out on paper. There is probably
            // a more clever way to do this.
            int nineRem;
            int nineDiv = Math.DivRem(coord, 9, out nineRem);
            int fourRem;
            int fourDiv = Math.DivRem(nineRem, 4, out fourRem);

            return new Point
            {
                X = this.MathModulo((-1 * fourDiv) + (2 * fourRem), 9),
                Y = (2 * nineDiv) + (nineRem - 1) / 4,
            };
        }

        /// <summary>
        /// Mathematical definition of modulo since % is just remainder
        /// </summary>
        /// <param name="dividend">Dividend</param>
        /// <param name="divisor">Divisor</param>
        /// <returns>Dividend modulo Divisor</returns>
        private int MathModulo(int dividend, int divisor)
        {
            return (Math.Abs(dividend * divisor) + dividend) % divisor;
        }
    }
}
