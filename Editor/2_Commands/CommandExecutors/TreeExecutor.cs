namespace Termule.Editor;

internal class TreeExecutor : CommandExecutor
{
    const int indentDepth = 2;

    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "tree",
        avaliableInsidePen = true
    };

    protected override void Execute()
    {
        string tree = GenerateBranch(pen.game.root, 0);
        Console.WriteLine(tree);
    }

    static string GenerateBranch(GameObject gameObject, int depth)
    {
        string branch = gameObject.name;
        string indent = GetIndent(depth + 1);     
        
        foreach (Component component in gameObject.components.Values)
        {
            branch += "\n" + indent;
            if (component is GameObject childGameObject)
            {
                branch += GenerateBranch(childGameObject, depth + 1);
            }
            else
            {
                branch += $"{component.name} ({component.GetType()})";
            }
        }

        return branch;
    }

    static string GetIndent(int depth)
    {
        string indent = null;
        for (int i = 0; i < depth * indentDepth; i++)
        {
            indent += " ";
        }

        return indent;
    }
}