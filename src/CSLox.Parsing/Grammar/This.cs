using System.Text;
using CSLox.Core.Exceptions;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record This(Token Keyword) : Expr
{
    public override void Resolve()
    {
        if (Resolver.CurrentScopeType != Enums.ScopeType.Method)
        {
            throw new LoxException("Invalid use of 'this'", "Can't use 'this' keyword outside of a class");
        }

        Resolver.ResolveLocalVariable(this, Keyword);
    }

    public override object Interpret()
    {
        return Interpreter.GetVariable("this", this);
    }

    internal override (int counter, string content) Draw()
    {
        s_counter++;
        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append("color=green;");
            sb.Append("node[style=filled];");
            sb.Append($"label=\"This\";");
            sb.Append($"expr_{s_counter}[label=\"this\"];");
        sb.Append('}');

        return (s_counter, sb.ToString());
    }
}