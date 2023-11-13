using System.Text;
using Godot;

namespace SpookyBotanyGame.Core;

public static class Node2DExtensions
{
    public static string PrintDebugName(this Node node, int depth)
    {
        if (node.IsInsideTree())
        {
            Node current = node;
            StringBuilder sb = new StringBuilder();
            string[] nodeNames = new string[depth];
            for (int i = 0; i < depth; i++)
            {
                if (current == null)
                {
                    break;
                }
                current = current.GetParent();
                if (current != null)
                {
                    nodeNames[i - depth] = current.Name;
                }
            }

            sb.AppendJoin(".", nodeNames);
            return sb.ToString();
        }

        return node.Name;
    }
}