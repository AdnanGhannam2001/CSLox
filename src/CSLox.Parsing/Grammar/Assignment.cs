using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Assignment(Token Name, Expr Value) : Expr
{
    public override object Interpret()
    {
        var value = Value.Interpret();
        Parser.s_environment.Assign(Name.Lexeme, Value.Interpret());
        return value;
    }

    internal override (int counter, string content) Draw()
    {
        throw new NotImplementedException();
    }
}