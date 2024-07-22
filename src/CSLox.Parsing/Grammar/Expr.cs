using System.Text;

namespace CSLox.Parsing.Grammar;

public abstract record Expr()
{
    protected static int s_counter = 0;

    internal abstract (int counter, string content) Draw();
    public abstract void Resolve();
    public abstract object Interpret();

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

    public static bool Truthy(object obj)
    {
        if (obj is Literal.Null) return false;
        if (obj is bool b) return b;
        if (string.IsNullOrEmpty(obj.ToString())) return false;
        if (int.TryParse(obj.ToString(), out var i)) return i != 0;
        return true;
    }

    public override int GetHashCode() => base.GetHashCode() ^ 9;
}