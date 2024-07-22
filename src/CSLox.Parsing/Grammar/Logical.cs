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
        // TODO
        throw new NotImplementedException();
    }
}