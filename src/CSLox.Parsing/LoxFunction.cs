using CSLox.Parsing.Exceptions;
using CSLox.Parsing.Grammar;
using CSLox.Parsing.Interfaces;

namespace CSLox.Parsing;

internal sealed class LoxFunction : ICallable
{
    private readonly FunctionStatement _declaration;
    private readonly Parser.Environment _environment;

    public LoxFunction(FunctionStatement declaration, Parser.Environment environment)
    {
        _declaration = declaration;
        _environment = environment;
    }

    public object? Invoke(IList<object> arguments)
    {
        ((ICallable) this).CheckArity(_declaration.Parameters.Count, arguments.Count);

        var environment = new Parser.Environment(_environment);

        for (var i = 0; i < arguments.Count; ++i)
        {
            environment.Decalare(_declaration.Parameters[i].Lexeme, arguments[i]);
        }

        try
        {
            Block.Execute(_declaration.Body, environment);
            return null;
        }
        catch (ReturnException exp)
        {
            return exp.Value;
        }
    }

    public override string ToString()
    {
        return $"<fn {_declaration.Name.Lexeme}>";
    }
}