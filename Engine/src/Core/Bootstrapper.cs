namespace Termule.Internals;

static partial class Bootstrapper
{
    //args[0] = project dll location
    static void Main(string[] args)
    {
        _ = new Game(args[0]);
    }
}