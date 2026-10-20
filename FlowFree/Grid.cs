public class Grid
{
    public int GridDimension {get; }
    private GameColors[,] _cells;
    
    public Grid()
    {
        GridDimension = 8;
        _cells = new GameColors[GridDimension,GridDimension];
    }

    /// <summary>
    /// Set a color in a specified position.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="color"></param>
    public void SetColor(Position pos, GameColors color)
    {
        _cells[pos.Row, pos.Column] = color;
    }

    /// <summary>
    /// Return a cell color given his position.
    /// </summary>
    /// <returns></returns>
    public GameColors ReturnColorFromPosition(Position pos)
    {
        return _cells[pos.Row,pos.Column];
    } 

    /// <summary>
    /// Set the cell color at his default.
    /// </summary>
    public void ResetSingleCell(Position pos)
    {
        SetColor(pos, GameColors.No_Color);
    }

    /// <summary>
    /// Check if whole grid is full by checking if all the position have a color.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfGridIsFull()
    {
        for (int i = 0; i < GridDimension; i++)
        {
            for (int j = 0; j < GridDimension; j++)
            {
                if(_cells[i,j] == GameColors.No_Color) return false;
            }
        }

        return true;
    }
}