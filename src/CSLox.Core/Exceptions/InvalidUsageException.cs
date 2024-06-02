namespace CSLox.Core.Exceptions;

public class InvalidUsageException : LoxException
{
    private static readonly string Usage = "Usage: program_name [script]";
    
    public InvalidUsageException()
        : base("Invalid Usage", Usage) { }
}