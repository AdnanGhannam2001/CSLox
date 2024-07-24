using CSLox.Scanning.Enums;

namespace CSLox.Scanning;

public sealed record Token(TokenType Type, string Lexeme, object? Literal, int Line, int Start, int Position, int Length)
{
    public override string ToString()
    {
        return $"({Type})\t'{Lexeme}'\tat line: {Line}, columns: {Start}-{Length + Start}";
    }
}
