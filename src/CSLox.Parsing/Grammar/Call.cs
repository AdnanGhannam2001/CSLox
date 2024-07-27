using System.Text;
using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing.Grammar;

public record Call(Expr Callee, IList<Expr> Arguments) : Expr
{
    public const int MAX_ARGUMENTS = byte.MaxValue;

    public override void Resolve()
    {
        Callee.Resolve();
        foreach (var argument in Arguments)
        {
            argument.Resolve();
        }
    }

    public override object Interpret()
    {
        var callee = Callee.Interpret();

        var arguments = new List<object>();

        foreach (var argument in Arguments)
        {
            arguments.Add(argument.Interpret());
        }

        if (callee is not ICallable function)
        {
            throw new RuntimeException($"Only Classes & Functions can be called");
        }

        return function.Invoke(arguments) ?? Literal.NULL;
    }

    internal override (int counter, string content) Draw()
    {
        var currentCounter = ++s_counter;

        var sb = new StringBuilder();

        sb.Append($"subgraph cluster_{s_counter}");
        sb.Append('{');
            sb.Append($"label=\"Call\";");
            sb.Append($"expr_{s_counter}[label=\"()\"];");

            var (calleeExprCounter, calleeContent) = Callee.Draw();
            sb.Append($"expr_{currentCounter}->expr_{calleeExprCounter};");
            sb.Append(calleeContent);

            sb.Append($"expr_{currentCounter}->expr_{s_counter + 1};");
            currentCounter = ++s_counter;
            sb.Append($"subgraph cluster_{currentCounter}");
            sb.Append('{');
                sb.Append("color=green;");
                sb.Append("node[style=filled];");
                sb.Append($"label=\"Arguments\";");
                sb.Append($"expr_{currentCounter}[label=\"[]\"];");

                var oldCounter = currentCounter;
                foreach (var arg in Arguments)
                {
                    currentCounter = ++s_counter;
                    var (argExprCounter, argContent) = arg.Draw();
                    sb.Append($"expr_{oldCounter}->expr_{argExprCounter};");
                    sb.Append(argContent);
                }
            sb.Append('}');
        sb.Append('}');

        return (currentCounter, sb.ToString());
    }
}