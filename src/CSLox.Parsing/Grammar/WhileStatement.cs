namespace CSLox.Parsing.Grammar;

public record WhileStatement(Expr Condition, Statement Body) : Statement
{
    public override void Resolve()
    {
        Condition.Resolve();
        Body.Resolve();
    }

    public override void Interpret()
    {
        while (Expr.Truthy(Condition.Interpret()))
        {
            Body.Interpret();
        }
    }
}