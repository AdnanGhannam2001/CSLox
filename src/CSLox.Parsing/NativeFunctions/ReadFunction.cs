using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing.NativeFunctions;

internal sealed class ReadFunction : ICallable
{
    public object? Invoke(IList<object> arguments)
    {
        ((ICallable) this).CheckArity(0, arguments.Count);

        return Console.ReadLine();
    }
}