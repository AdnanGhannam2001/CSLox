namespace CSLox.Parsing.Grammar;

public record Block(IList<Statement> Statements) : Statement
{
    public override void Interpret()
    {
        Execute(Statements, new Parser.Environment(Parser.s_environment));
    }

    internal static void Execute(IList<Statement> statements, Parser.Environment environment)
    {
        Parser.Environment prev = Parser.s_environment;

        Parser.s_environment = environment;

        try
        {
            foreach (var statement in statements)
            {
                statement.Interpret();
            }
        }
        finally
        {
            Parser.s_environment = prev;
        }
    }
}
