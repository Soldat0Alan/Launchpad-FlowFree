public class Level
{
    public int LevelId {get;}
    public List<EndpointPair> EndpointPairs;
    
    public Level(int levelId, List<EndpointPair> pairs)
    {
        LevelId = levelId;
        EndpointPairs = pairs;
    }
}