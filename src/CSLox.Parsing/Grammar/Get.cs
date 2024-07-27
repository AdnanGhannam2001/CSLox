using System.Text;
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
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{currentCounter}");
        sb.Append('{');
            sb.Append($"label=\"Get\";");
            sb.Append($"expr_{currentCounter}[label=\".\"];");

            var (objectExprCounter, objectContent) = Object.Draw();
            sb.Append($"expr_{currentCounter}->expr_{objectExprCounter};");
            sb.Append(objectContent);

            sb.Append($"expr_{currentCounter}->expr_{++s_counter};");
            sb.Append($"subgraph cluster_{s_counter}");
            sb.Append('{');
                sb.Append("color=green;");
                sb.Append("node[style=filled];");
                sb.Append($"label=\"Field/Method\";");
                sb.Append($"expr_{s_counter}[label=\"{Field.Lexeme}\"];");
            sb.Append('}');
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}