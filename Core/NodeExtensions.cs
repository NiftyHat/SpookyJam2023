using Godot;

namespace SpookyBotanyGame.Core;

public static class NodeExtensions
{
    /// <summary>
    /// Searches through all children of a node looking for the first child matching the given T type.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="recursive"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindFirstChild<T>(this Node node, bool recursive = true) where T : Node
    {
        int childCount = node.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            Node child = node.GetChild(i);

            if (child is T childT)
            {
                return childT;
            }
            
            int len = child.GetChildCount();
            if (recursive && child.GetChildCount() > 0)
            {
                for (int index = 0; i < len; i++)
                {
                    T recursiveResult = child.GetChildOrNull<T>(index);
                    if (recursiveResult != null)
                    {
                        return recursiveResult;
                    }
                }
            }
        }
        return null;
    }
}