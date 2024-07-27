using System.Diagnostics;
using System.Text;
using CSLox.Parsing.Exceptions;
using CSLox.Scanning;
using static CSLox.Scanning.Enums.TokenType;

namespace CSLox.Parsing.Grammar;

public record Binary(Expr LeftExpression, Token Operator, Expr RightExpression) : Expr
{
    public override void Resolve()
    {
        LeftExpression.Resolve();
        RightExpression.Resolve();
    }

    public override object Interpret()
    {
        var rvalue = RightExpression.Interpret();
        var lvalue = LeftExpression.Interpret();

        switch (Operator.Type)
        {
            case PLUS:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl + dr;
                    else if (lvalue is string sl && rvalue is string sr) return sl + sr;

                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number' or 'string'");
                }
            case MINUS:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl - dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }
            case STAR:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl * dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }
            case SLASH:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl / dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }

            case GREATER:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl > dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }
            case GREATER_EQUAL:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl >= dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }
            case LESS:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl < dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }
            case LESS_EQUAL:
                {
                    if (lvalue is double dl && rvalue is double dr) return dl <= dr;
                    throw new RuntimeException("Expected lvalue & rvalue to be both 'number'");
                }

            case BANG_EQUAL: { return !lvalue.Equals(rvalue); }
            case EQUAL_EQUAL: { return lvalue.Equals(rvalue); }
        }

        throw new UnreachableException();
    }

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