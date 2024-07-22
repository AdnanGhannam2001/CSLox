#if PRINT_AST
using CSLox.Core.Common;
#endif

namespace CSLox.Parsing.Grammar;

public record ExpressionStatement(Expr Expression) : Statement
{
    public override void Resolve()
    {
        Expression.Resolve();
    }

    public override void Interpret()
    {
        Expression.Interpret();

#if PRINT_AST
        Logger.LogDebug("AST", Expression.Display());
#endif
    }
}