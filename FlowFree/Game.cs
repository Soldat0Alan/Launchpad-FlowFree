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

        foreach (EndpointPair endpointPair in level.EndpointPairs)
        {
            PlayerPaths.Add(new Path(endpointPair));
            
            // Imposta i colori iniziali degli estremi sulla griglia
            Grid.SetColor(endpointPair.Start, endpointPair.Color);
            Grid.SetColor(endpointPair.End, endpointPair.Color);
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
                Grid.SetColor(endpointPair.Start, endpointPair.Color);
                Grid.SetColor(endpointPair.End, endpointPair.Color);
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

        // It permits to change the enpoint when antoher one of the same path is already selected.
        if (IsTargetEndpoint(pos, SelectedColor))
        {
            SupportFunctionForExtendPath(path,pos);
            
        }

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
                SupportFunctionForExtendPath(path, pos);
            }
        } 

    }

    /// <summary>
    /// A function that can be reused inside the ExtendPath func to have less repretitions.
    /// </summary>
    /// <returns></returns>
    public void SupportFunctionForExtendPath(Path path, Position pos)
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

    public void Press(Position pos)
    {
        if (IsGameWon) return;

        if (SelectedColor == GameColors.No_Color)
        {
            StartPath(pos);
        }
        else
        {
            Path currentPath = GetPathByColor(SelectedColor);

            if (IsAnyEndpoint(pos) && !IsTargetEndpoint(pos, SelectedColor))
            {
                StartPath(pos);
            }
            else
            {
                ExtendPath(pos);
                Console.WriteLine("fine");
            }
        }
    }


    private bool IsAnyEndpoint(Position pos)
    {
        return Level.EndpointPairs.Any(pair => pair.Start.Equals(pos) || pair.End.Equals(pos));
    }

    private bool IsTargetEndpoint(Position pos, GameColors color)
    {
        var pair = Level.EndpointPairs.FirstOrDefault(p => p.Color == color);
        return pair != null && (pair.Start.Equals(pos) || pair.End.Equals(pos));
    }

}