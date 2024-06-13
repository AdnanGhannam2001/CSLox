#if PRINT_AST
using CSLox.Core.Common;
#endif

namespace CSLox.Parsing.Grammar;

public record PrintStatement(Expr Expression) : Statement
{
    public override void Interpret()
    {
        var value = Expression.Interpret();

#if PRINT_AST
        Logger.LogDebug("AST", Expression.Display());
#endif

        Console.WriteLine(value);
    }
}