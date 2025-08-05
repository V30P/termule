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
        string tree = GenerateBranch(pen.game, 0);
        Console.WriteLine(tree);
    }

    static string GenerateBranch(IComposite composite, int depth)
    {
        string branch = (composite as Component)?.name;
        string indent = GetIndent(depth);     
        
        foreach (Component component in composite)
        {
            branch += "\n" + indent;
            if (component is IComposite containedComposite)
            {
                branch += GenerateBranch(containedComposite, depth + 1);
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