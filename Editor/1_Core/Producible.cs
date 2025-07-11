namespace Termule.Editor;

internal abstract class NongenericProducible
{
    internal string[] args;
}

internal abstract class Producible<TInfo> : NongenericProducible where TInfo : ProducibleInfo
{
    internal abstract TInfo info { get; }
}
