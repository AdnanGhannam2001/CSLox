using System.Text;
using CSLox.Core.Common;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Variable(Token Identifier) : Expr
{
    public override void Resolve()
    {
        if (Resolver.NotInitialized(Identifier.Lexeme))
        {
            Logger.LogError("Invalid access", $"Trying to access an uninitialized variable '{Identifier.Lexeme}'");
            return;
        }

        Resolver.ResolveLocalVariable(this, Identifier);
    }

    public override object Interpret()
    {
        return Interpreter.GetVariable(Identifier.Lexeme, this);
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
            sb.Append($"expr_{s_counter}[label=\"{Identifier.Lexeme}\"];");
        sb.Append('}');

        return (s_counter, sb.ToString());
    }
}
