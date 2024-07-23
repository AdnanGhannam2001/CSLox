using CSLox.Parsing.Exceptions;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Set(Expr Object, Token Field, Expr Value) : Expr
{
    public override void Resolve()
    {
        Object.Resolve();
        Value.Resolve();
    }

    public override object Interpret()
    {
        var @object = Object.Interpret() as LoxObject
            ?? throw new RuntimeException("Only objects has fields");
        @object.SetField(Field.Lexeme, Value.Interpret());

        return Value;
    }

    internal override (int counter, string content) Draw()
    {
        // TODO
        throw new NotImplementedException();
    }
}