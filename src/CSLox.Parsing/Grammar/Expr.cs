namespace CSLox.Parsing.Grammar;

public abstract record Expr()
{
    protected static int s_counter = 0;
    internal abstract (int counter, string content) Draw();
    public string Display()
    {
        return Draw().content;
    }
}