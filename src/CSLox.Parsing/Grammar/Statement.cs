namespace CSLox.Parsing.Grammar;

public abstract record Statement
{
    public abstract void Interpret();
}