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
        throw new NotImplementedException();
    }
}