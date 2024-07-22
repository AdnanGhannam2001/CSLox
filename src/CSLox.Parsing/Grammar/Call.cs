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
        // TODO
        throw new NotImplementedException();
    }
}