using CSLox.Core.Common;

namespace CSLox.Core.Exceptions;

public class LoxException : Exception
{
    public LoxException(string message, string? details = null) : base(message)
    {
        Details = details;
    }

    public string? Details { get; init; }
}