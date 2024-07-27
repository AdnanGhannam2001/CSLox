using System.Text;
using CSLox.Scanning;
using CSLox.Scanning.Enums;

namespace CSLox.Parsing.Grammar;

public record Logical(Expr Left, Token Operator, Expr Right) : Expr
{
    public override void Resolve()
    {
        Left.Resolve();
        Right.Resolve();
    }

    public override object Interpret()
    {
        var left = Left.Interpret();

        if (Operator.Type == TokenType.OR)
        {
            if (Truthy(left)) return left;
        }
        else
        {
            if (!Truthy(left)) return left;
        }

        return Right.Interpret();
    }

    internal override (int counter, string content) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Logical\";");
            sb.Append($"expr_{s_counter}[label=\"{Operator.Lexeme}\"];");

            var (leftExprCounter, leftContent) = Left.Draw();
            sb.Append($"expr_{currentCounter}->expr_{leftExprCounter};");
            sb.Append(leftContent);

            var (rightExprCounter, rightContent) = Right.Draw();
            sb.Append($"expr_{currentCounter}->expr_{rightExprCounter};");
            sb.Append(rightContent);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}