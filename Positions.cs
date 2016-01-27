namespace MySnake
{
    using System;

    public struct Positions
    {
        public Positions(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public int Row { get; set; }

        public int Col { get; set; }
    }
}
