namespace CSLox.Parsing.Grammar;

public record ForStatement(Statement? Init, Expr? Condition, Expr? SideEffect, Statement Body) : Statement
{
    public override void Interpret()
    {
        for (Init?.Interpret(); Condition is null || Expr.Truthy(Condition.Interpret()); SideEffect?.Interpret())
        {
            Body.Interpret();
        }
    }
}