using Termule.Engine.Types;

namespace Termule.Engine.Systems.Input;

internal sealed class MouseMoved(VectorInt pos) : InputMessage
{
    internal VectorInt Pos { get; init; } = pos;
}
