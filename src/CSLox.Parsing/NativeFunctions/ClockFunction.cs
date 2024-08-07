using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing.NativeFunctions;

internal sealed class ClockFunction : ICallable
{
    public object Invoke(IList<object> arguments)
    {
        ((ICallable) this).CheckArity(0, arguments.Count);

        return (double) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public override string ToString()
    {
        return "<native fn>";
    }
}