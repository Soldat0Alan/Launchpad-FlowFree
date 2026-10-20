using System.Collections.Generic;
public class Path
{
    public EndpointPair EndpointPair { get; }
    public GameColors Color => EndpointPair.Color;
    public List<Position> Cells { get; } = new List<Position>();

    public Path(EndpointPair endpointPair)
    {
        EndpointPair = endpointPair;
    }

    /// <summary>
    /// Add a cell next to the previous one.
    /// </summary>
    /// <param name="pos"></param>
    public void Extend(Position pos)
    {
        Cells.Add(pos);
    }

    /// <summary>
    /// Check if the position makes part of the Path.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool Contains(Position pos)
    {
        return Cells.Contains(pos);
    }

    /// <summary>
    /// Returns the last position of a Path.
    /// </summary>
    /// <returns></returns>
    public Position LastPosition()
    {
        return Cells[Cells.Count - 1];
    }

    /// <summary>
    /// Shortens the path so that it ends at the given position if its cut by another path.
    /// </summary>
    /// <param name="pos"></param>
    public void CutPathTo(Position pos)
    {
        int index = Cells.IndexOf(pos);
        if (index == -1) return;

        while (Cells.Count > index + 1)
        {
            Cells.RemoveAt(Cells.Count - 1);
        }
    }

    /// <summary>
    /// Clear all the cell of a path.
    /// </summary>
    public void ClearPath()
    {
        Cells.Clear();
    }

    /// <summary>
    /// Check if a path is completed.
    /// </summary>
    /// <returns></returns>
    public bool IsCompleted()
    {
        if (Cells.Count < 2) return false;

        Position first = Cells[0];
        Position last = Cells[Cells.Count - 1];

        bool firstIsEndpoint = first.Equals(EndpointPair.Start) || first.Equals(EndpointPair.End);
        bool lastIsEndpoint = last.Equals(EndpointPair.Start) || last.Equals(EndpointPair.End);

        return firstIsEndpoint && lastIsEndpoint;
    }
}