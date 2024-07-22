namespace CSLox.Parsing.Grammar;

public record IfStatement(Expr Condition, Statement Then, Statement? Else) : Statement
{
    public override void Resolve()
    {
        Condition.Resolve();
        Then.Resolve();
        Else?.Resolve();
    }

    public override void Interpret()
    {
        if (Expr.Truthy(Condition.Interpret()))
        {
            Then.Interpret();
        }
        else
        {
            Else?.Interpret();
        }
    }
}