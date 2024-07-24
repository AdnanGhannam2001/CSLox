using CSLox.Parsing.Exceptions;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record Get(Expr Object, Token Field) : Expr
{
    public override void Resolve()
    {
        Object.Resolve();
    }

    public override object Interpret()
    {
        var @object = Object.Interpret() as LoxObject ??
            throw new RuntimeException("Only objects has fields");

        return @object.Get(Field.Lexeme)
            ?? throw new RuntimeException(
                $"Unknown field or method: '{Field.Lexeme}' at line: {Field.Line}, columns: {Field.Start}-{Field.Length + Field.Start}");
    }

    internal override (int counter, string content) Draw()
    {
        // TODO
        throw new NotImplementedException();
    }
}