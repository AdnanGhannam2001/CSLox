using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;

namespace CSLox.Parsing.Grammar;

public record Block(IList<Statement> Statements) : Statement
{
    public override void Resolve()
    {
        Resolver.BeginScope();
            Resolver.Resolve(Statements);
        Resolver.EndScope();
    }

    public override void Interpret()
    {
        Execute(Statements, new Interpreting.Environment(Interpreter.Environment));
    }

    internal static void Execute(IList<Statement> statements, Interpreting.Environment environment)
    {
        var prev = Interpreter.Environment;
        Interpreter.Environment = environment;

        try
        {
            foreach (var statement in statements)
            {
                statement.Interpret();
            }
        }
        finally
        {
            Interpreter.Environment = prev;
        }
    }
}
