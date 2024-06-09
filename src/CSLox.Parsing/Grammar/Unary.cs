using System.Diagnostics;
using System.Text;
using CSLox.Parsing.Exceptions;
using CSLox.Scanning;
using static CSLox.Scanning.Enums.TokenType;

namespace CSLox.Parsing.Grammar;

public record Unary(Token Operator, Expr Expression) : Expr
{
    internal override (int, string) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Unary\";");
            sb.Append($"expr_{s_counter}[label=\"{Operator.Lexeme}\"];");

            var (exprCounter, content) = Expression.Draw();
            sb.Append($"expr_{currentCounter}->expr_{exprCounter};");
            sb.Append(content);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }

    public override object Interpret()
    {
        var value = Expression.Interpret();

        switch (Operator.Type)
        {
            case BANG: { return !Truthy(value); }
            case MINUS:
                {
                    if (value is double v) return -v;
                    throw new RuntimeException("Expected value after '-' to be 'number'");
                }
        }

        throw new UnreachableException();
    }

    private static bool Truthy(object? obj)
    {
        if (obj is null) return false;
        if (obj is bool b) return b;
        return true;
    }
}