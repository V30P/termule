namespace Termule.Saddlebag;

[AttributeUsage(AttributeTargets.Class)]
internal class ExecutorAttribute(string command) : Attribute
{
    internal readonly string command = command;
}