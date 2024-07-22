namespace CSLox.Parsing.Grammar;

public abstract record Statement
{
    public abstract void Resolve();
    public abstract void Interpret();
}