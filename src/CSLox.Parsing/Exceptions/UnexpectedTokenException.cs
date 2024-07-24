using System.Text;
using CSLox.Core.Exceptions;
using CSLox.Scanning;

namespace CSLox.Parsing.Exceptions;

public class UnexpectedTokenException : LoxException
{
    private static string GetDetails(string content, Token unexpectedToken, string expected)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"{unexpectedToken.Line} | ");
        int lineNumberLength = stringBuilder.Length;

        var start = unexpectedToken.Position;
        while (start > 0 && content[--start] != '\n') ;

        var end = unexpectedToken.Position;
        while (end < content.Length - 1 && content[++end] != '\n') ;

        stringBuilder.AppendLine(content[start..end].Trim('\n'));
        stringBuilder.Append(' ', lineNumberLength + unexpectedToken.Start - 1);
        stringBuilder.Append('^', unexpectedToken.Length + 1);
        stringBuilder.AppendLine($" expected {expected}");

        return stringBuilder.ToString();
    }

    public UnexpectedTokenException(string content, Token unexpectedToken, string expected)
        : base(
            $"Unexpected Token '{unexpectedToken.Lexeme}' of type '{unexpectedToken.Type}' at line: {unexpectedToken.Line}, column: {unexpectedToken.Start}-{unexpectedToken.Length + unexpectedToken.Start}",
            GetDetails(content, unexpectedToken, expected))
    { }
}