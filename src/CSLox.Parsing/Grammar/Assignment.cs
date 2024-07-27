using System.Text;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Assignment(Token Name, Expr Value) : Expr
{
    public override void Resolve()
    {
        Value.Resolve();
        Resolver.ResolveLocalVariable(this, Name);
    }

    public override object Interpret()
    {
        var value = Value.Interpret();
        Interpreter.SetVariable(Name.Lexeme, Value.Interpret(), this);
        return value;
    }

    internal override (int counter, string content) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Binary\";");
            sb.Append($"expr_{s_counter}[label=\"=\"];");

            sb.Append($"expr_{currentCounter}->expr_{++s_counter};");
            sb.Append($"subgraph cluster_{s_counter}");
            sb.Append('{');
                sb.Append("color=green;");
                sb.Append("node[style=filled];");
                sb.Append($"label=\"Variable\";");
                sb.Append($"expr_{s_counter}[label=\"{Name.Lexeme}\"];");
            sb.Append('}');

            var (valueExprCounter, valueContent) = Value.Draw();
            sb.Append($"expr_{currentCounter}->expr_{valueExprCounter};");
            sb.Append(valueContent);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}