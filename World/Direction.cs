namespace SpookyBotanyGame.World;

public class Direction
{
    public Direction(string name)
    {
        Name = name;
    }
    public readonly string Name;
    
    public static readonly Direction North = new Direction("North");
    public static readonly Direction South = new Direction("South");
    public static readonly Direction East = new Direction("East");
    public static readonly Direction West = new Direction("West");
    
    public static readonly Direction NorthEast = new Direction("NorthEast");
    public static readonly Direction SouthEast = new Direction("SouthEast");
    public static readonly Direction NorthWest = new Direction("NorthWest");
    public static readonly Direction SouthWest = new Direction("SouthWest");
}