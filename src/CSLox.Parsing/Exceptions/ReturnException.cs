namespace CSLox.Parsing.Exceptions;

internal class ReturnException : Exception
{
    public ReturnException(object value)
    {
        Value = value;
    }

    public object Value { get; }
}