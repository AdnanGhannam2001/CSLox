using System.Text;

namespace CSLox.Parsing.Grammar;

public record Literal(object? Value) : Expr
{
    internal struct Null
    {
        public override string ToString()
        {
            return "nil";
        }
    }

    internal static readonly Null NULL = new();

    public override void Resolve() { }

    internal override (int, string) Draw()
    {
        s_counter++;
        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append("color=green;");
            sb.Append("node[style=filled];");
            sb.Append($"label=\"Literal\";");
            sb.Append($"expr_{s_counter}[label=\"{Value}\"];");
        sb.Append('}');

        return (s_counter, sb.ToString());
    }

    public override object Interpret()
    {
        return Value ?? NULL;
    }
}