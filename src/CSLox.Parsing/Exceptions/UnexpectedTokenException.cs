using CSLox.Core.Exceptions;
using CSLox.Scanning;

namespace CSLox.Parsing.Exceptions;

public class UnexpectedTokenException : LoxException
{
    public UnexpectedTokenException(Token token, string? details = null)
        : base($"Unexpected Token '{token.Lexeme}' of type '{token.Type}' at line: {token.Line}, column: {token.Start}-{token.End}", details) { }
}