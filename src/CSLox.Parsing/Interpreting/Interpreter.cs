using CSLox.Parsing.Grammar;
using CSLox.Parsing.NativeFunctions;

using ResolveMap = System.Collections.Generic.Dictionary<CSLox.Parsing.Grammar.Expr, int>;

namespace CSLox.Parsing.Interpreting;

public static class Interpreter
{
    private static readonly Environment Globals = NativeFunctionsFactory.Build();

    internal static Environment Environment = Globals;
    internal static readonly ResolveMap ResolveMap = [];

    internal static void Resolve(Expr expression, int depth)
    {
        ResolveMap.Add(expression, depth);
    }

    internal static object GetVariable(string name, Expr expression)
    {
        if (ResolveMap.TryGetValue(expression, out var distance))
        {
            return Environment.GetVariableValue(name, distance);
        }

        return Globals.GetVariableValue(name) ?? Literal.NULL;
    }
    
    internal static void SetVariable(string name, object value, Expr expression)
    {
        if (ResolveMap.TryGetValue(expression, out var distance))
        {
            Environment.Assign(name, value, distance);
            return;
        }

        Globals.Assign(name, value);
    }
    
    public static void Interpret(IEnumerable<Statement> statements)
    {
        foreach (var statement in statements)
        {
            statement.Interpret();
        }
    }
}