namespace SpookyBotanyGame.World;

public class Direction
{
    public Direction(string name)
    {
        Name = name;
    }
    public readonly string Name;
    
    public static Direction North = new Direction("North");
    public static Direction South = new Direction("South");
    public static Direction East = new Direction("East");
    public static Direction West = new Direction("West");
}