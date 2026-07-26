using Termule.Engine.Types;

namespace Termule.Engine.Systems.Input;

internal sealed class MouseMoved(VectorInt Pos) : InputMessage
{
    internal VectorInt Pos { get; init; } = Pos;
}
