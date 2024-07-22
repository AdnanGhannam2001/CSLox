using CSLox.Core.Exceptions;
using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Resolving;

namespace CSLox.Parsing.Grammar;

public record ReturnStatement(Expr? Expression) : Statement
{
    public override void Resolve()
    {
        if (Resolver.CurrentScopeType == Enums.ScopeType.None)
        {
            throw new LoxException("Invalid use of return", "Return statements can only be inside functions");
        }

        Expression?.Resolve();
    }

    public override void Interpret()
    {
        object? value;
        if ((value = Expression?.Interpret()) is not null)
        {
            throw new ReturnException(value);
        }
    }
}