public class EndpointPair
{
    public GameColors Color { get; }
    public Position Start { get; }
    public Position End { get; }

    public EndpointPair(GameColors color, Position posStart, Position posEnd)
    {
        Color = color;
        Start = posStart;
        End = posEnd;
    }
}