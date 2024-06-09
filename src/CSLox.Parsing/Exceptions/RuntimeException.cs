using CSLox.Core.Exceptions;

namespace CSLox.Parsing.Exceptions;

public class RuntimeException : LoxException
{
    public RuntimeException(string? details = null) : base("Runtime Error", details)
    {
    }
}