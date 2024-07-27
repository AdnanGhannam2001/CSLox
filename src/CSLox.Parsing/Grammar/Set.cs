using System.Text;
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
        @object.Set(Field.Lexeme, Value.Interpret());

        return Value;
    }

    internal override (int counter, string content) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Binary\";");
            sb.Append($"expr_{s_counter}[label=\"=\"];");

            var objectExpressionCounter = ++s_counter;
            sb.Append($"expr_{currentCounter}->expr_{objectExpressionCounter};");
            sb.Append($"subgraph cluster_{objectExpressionCounter}");
            sb.Append('{');
                sb.Append("color=green;");
                sb.Append("node[style=filled];");
                sb.Append($"label=\"Get\";");
                sb.Append($"expr_{objectExpressionCounter}[label=\".\"];");

                var (objectExprCounter, objectContent) = Object.Draw();
                sb.Append($"expr_{objectExpressionCounter}->expr_{objectExprCounter};");
                sb.Append(objectContent);

                sb.Append($"expr_{objectExpressionCounter}->expr_{++s_counter};");
                sb.Append($"subgraph cluster_{s_counter}");
                sb.Append('{');
                    sb.Append("color=green;");
                    sb.Append("node[style=filled];");
                    sb.Append($"label=\"Field/Method\";");
                    sb.Append($"expr_{s_counter}[label=\"{Field.Lexeme}\"];");
                sb.Append('}');
            sb.Append('}');

            var (valueExprCounter, valueContent) = Value.Draw();
            sb.Append($"expr_{currentCounter}->expr_{valueExprCounter};");
            sb.Append(valueContent);
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}