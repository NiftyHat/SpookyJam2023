namespace SpookyBotanyGame.World;

public class Direction
{
    public Direction(string name)
    {
        Name = name;
    }
    public readonly string Name = "";
    
    public static Direction North = new Direction("north");
    public static Direction South = new Direction("south");
    public static Direction East = new Direction("east");
    public static Direction West = new Direction("west");
}