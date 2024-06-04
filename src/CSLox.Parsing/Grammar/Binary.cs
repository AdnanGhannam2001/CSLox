using System.Text;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Binary(Expr LeftExpression, Token Operator, Expr RightExpression) : Expr
{
    internal override (int, string) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Binary\";");
            sb.Append($"expr_{s_counter}[label=\"{Operator.Lexeme}\"];");

            var (leftExprCounter, leftContent) = LeftExpression.Draw();
            sb.Append($"expr_{currentCounter}->expr_{leftExprCounter};");
            sb.Append(leftContent);

            var (rightExprCounter, rightContent) = RightExpression.Draw();
            sb.Append($"expr_{currentCounter}->expr_{rightExprCounter};");
            sb.Append(rightContent);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}