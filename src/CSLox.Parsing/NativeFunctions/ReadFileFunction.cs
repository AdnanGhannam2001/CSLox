using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing.NativeFunctions;

internal sealed class ReadFileFunction : ICallable
{
    public object? Invoke(IList<object> arguments)
    {
        ((ICallable) this).CheckArity(1, arguments.Count);

        if (arguments.ElementAt(0) is not string path)
        {
            throw new RuntimeException("Argument to 'readfile' function should be a string");
        }

        try
        {
            return File.ReadAllText(path);
        }
        catch(Exception exp)
        {
            throw new RuntimeException(exp.Message);
        }
    }
}
