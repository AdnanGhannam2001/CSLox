using System.Text;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Variable(Token Identifier) : Expr
{
    public override object Interpret()
    {
        return Parser.s_environment.GetVariableValue(Identifier.Lexeme)!;
    }

    internal override (int counter, string content) Draw()
    {
        s_counter++;
        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append("color=green;");
            sb.Append("node[style=filled];");
            sb.Append($"label=\"Identifier\";");
            sb.Append($"expr_{s_counter}[label=\"{Identifier}\"];");
        sb.Append('}');

        return (s_counter, sb.ToString());
    }
}
