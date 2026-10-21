public class Game
{
    public Level Level {get; private set;}
    public bool IsGameWon { get; private set; }
    public GameColors SelectedColor { get; private set; }
    public Grid Grid { get; private set; }
    public List<Path> PlayerPaths { get; private set; }
    
    public Game(Level level)
    {
        Level = level;
        Grid = new Grid();
        IsGameWon = false;
        SelectedColor = GameColors.No_Color;
        PlayerPaths = new List<Path>();

        //Adds each ExtremePoints of a path inside the Game's grid.
        foreach (EndpointPair endpointPair in level.EndpointPairs)
        {
            PlayerPaths.Add(new Path(endpointPair));
        }
    }

    /// <summary>
    /// Given a Color returns the path of that color.
    /// </summary>
    /// <param name="colorSel"></param>
    /// <returns></returns>
    private Path GetPathByColor(GameColors colorSel)
    {
        return PlayerPaths.FirstOrDefault(p => p.Color == colorSel)!;
    }

    public void StartPath(Position pos)
    {
        //Checks if the position pos is one of the EndPoints
        foreach (EndpointPair endpointPair in Level.EndpointPairs)
        {
            //Check if they are the same points
            if(endpointPair.Start.Equals(pos) || endpointPair.End.Equals(pos))
            {
                //Retrieves the color of the clicked paths endpoints.
                SelectedColor = endpointPair.Color;

                //Saves inside the path the color the player has just decided to solve, by clicking one of the path's borders.
                Path path = GetPathByColor(SelectedColor);

                //Reset the current path (because the start or the end of the path was clicked).
                foreach (Position position in path.Cells)
                {
                    Grid.ResetSingleCell(position);
                }

                path.ClearPath();
                path.Extend(pos);
                Grid.SetColor(pos, SelectedColor);

                break;
            }
        }
    }

    private void CheckWin()
{
    foreach (Path p in PlayerPaths)
    {
        if (!p.IsCompleted()) return;
    }

    if (Grid.CheckIfGridIsFull())
    {
        IsGameWon = true;
    }
}

    public void ExtendPath(Position pos)
    {
        Path path = GetPathByColor(SelectedColor);

        //Check that the postion we need to add is near, in a Vertically or Horizontally way, to the Last path position  
        if (pos.IsNear(path.LastPosition()))
        {
            // Checking if the selected postion is already part of that path
            if (path.Contains(pos))
            {
                // Find and Reset the cells of the path assining the default color.
                int index = path.Cells.IndexOf(pos);
                for (int i = index + 1; i < path.Cells.Count; i++)
                {
                    Grid.ResetSingleCell(path.Cells[i]);
                }

                // Call the function to cut the other path (flow).
                path.CutPathTo(pos);
                return;
            }

            // Check if the pos is empty, if so it saves the new position and add it to the current path.
            GameColors cellColor = Grid.ReturnColorFromPosition(pos);
            if (cellColor == GameColors.No_Color || IsTargetEndpoint(pos, SelectedColor))
            {
                path.Extend(pos);
                Grid.SetColor(pos, SelectedColor);

                //Check if the path of that color is finished.
                if (path.IsCompleted())
                {
                    SelectedColor = GameColors.No_Color; 
                    CheckWin();
                }
            }
        }
    }

    private bool IsTargetEndpoint(Position pos, GameColors color)
    {
        var pair = Level.EndpointPairs.FirstOrDefault(p => p.Color == color);
        return pair != null && (pair.Start.Equals(pos) || pair.End.Equals(pos));
    }

}