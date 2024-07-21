using CSLox.Parsing.Exceptions;

namespace CSLox.Parsing.Grammar;

public record ReturnStatement(Expr? Expression) : Statement
{
    public override void Interpret()
    {
        object? value;
        if ((value = Expression?.Interpret()) is not null)
        {
            throw new ReturnException(value);
        }
    }
}