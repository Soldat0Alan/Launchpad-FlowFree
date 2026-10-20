public class Position
{
    private int _row, _column;

    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }

    /// <summary>
    /// Returns the Row number
    /// </summary>
    public int Row
    {
        get {return _row;}

        private set
        {
            if(value < 0 || value > 7)
            {
                throw new ArgumentException("The Row inserted doesn't exist.");
            }
            else
            {
                _row = value;
            }
        }
    }

    /// <summary>
    /// Returns the Column number
    /// </summary>
    public int Column
    {
        get {return _column;}

        private set
        {
            if(value < 0 || value > 7)
            {
                throw new ArgumentException("The Column inserted doesn't exist.");
            }
            else
            {
                _column = value;
            }
        }
    }

    /// <summary>
    /// Check if two positions are near (counts ONLY Vertical and Horizontal).
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsNear(Position pos)
    {
        if (pos == null) return false;

        int rowDifference = Math.Abs(Row - pos.Row);
        int colDifference = Math.Abs(Column - pos.Column);

        return (rowDifference + colDifference) == 1;
    }

    /// <summary>
    /// Check if two positions are equals.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        
        if(obj is Position pos)
        {
            if (pos.Column == Column && pos.Row == Row) return true;
        }
        
        return false;
    }

    /// <summary>
    /// Returns the objects HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
}