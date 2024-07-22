using System.Text;

namespace CSLox.Parsing.Grammar;

public record Grouping(Expr Expression) : Expr
{
    public override void Resolve()
    {
        Expression.Resolve();
    }

    internal override (int, string) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Grouping\";");
            sb.Append($"expr_{s_counter}[label=\"()\"];");

            var (exprCounter, content) = Expression.Draw();
            sb.Append($"expr_{currentCounter}->expr_{exprCounter};");
            sb.Append(content);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }

    public override object Interpret()
    {
        return Expression.Interpret();
    }
}