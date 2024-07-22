using CSLox.Parsing.Enums;
using CSLox.Parsing.Grammar;
using CSLox.Parsing.Interpreting;
using CSLox.Scanning;

namespace CSLox.Parsing.Resolving;

public static class Resolver
{
    private static readonly Stack<Dictionary<string, bool>> Scopes = new();
    internal static ScopeType CurrentScopeType = ScopeType.None;

    internal static bool IsGlobalScope => Scopes.Count == 0;

    internal static void BeginScope() => Scopes.Push([]);
    internal static void EndScope()   => Scopes.Pop();

    internal static bool NotInitialized(string name) => !IsGlobalScope
                                                        && Scopes.Peek().TryGetValue(name, out var value)
                                                        && value == false;

    internal static void Declare(string name)
    {
        if (IsGlobalScope) return;
        
        Scopes.Peek().Add(name, false);
    }

    internal static void Define(string name)
    {
        if (IsGlobalScope) return;

        Scopes.Peek()[name] = true;
    }

    internal static void ResolveLocalVariable(Expr expression, Token name)
    {
        for (var i = Scopes.Count - 1; i >= 0; --i)
        {
            if (Scopes.ElementAt(i).ContainsKey(name.Lexeme))
            {
                Interpreter.Resolve(expression, Scopes.Count - i - 1);
                return;
            }
        }
    }

    public static void Resolve(IEnumerable<Statement> statements)
    {
        foreach (var statement in statements)
        {
            statement.Resolve();
        }
    }
}