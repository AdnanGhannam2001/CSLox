using CSLox.Parsing.Grammar;
using CSLox.Parsing.NativeFunctions;

namespace CSLox.Parsing.Interpreting;

public static class Interpreter
{
    internal static Environment Environment = NativeFunctionsFactory.Build();
    
    public static void Interpret(IEnumerable<Statement> statements)
    {
        foreach (var statement in statements)
        {
            statement.Interpret();
        }
    }
}