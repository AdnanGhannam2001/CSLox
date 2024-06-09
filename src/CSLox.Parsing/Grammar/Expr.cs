using System.Text;

namespace CSLox.Parsing.Grammar;

public abstract record Expr()
{
    protected static int s_counter = 0;
    internal abstract (int counter, string content) Draw();
    public string Display()
    {
        s_counter = 0;
        var sb = new StringBuilder();

        sb.Append("digraph AST");
        sb.Append('{');
            sb.Append(Draw().content);
        sb.Append('}');

        return sb.ToString();
    }
}