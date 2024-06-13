using CSLox.Parsing.Exceptions;

namespace CSLox.Parsing;

internal static class Environment
{
    private static readonly Dictionary<string, object?> Declarations = [];

    public static void Decalare(string name, object? value)
    {
        if (Declarations.ContainsKey(name))
        {
            Declarations[name] = value;
            return;
        }

        Declarations.Add(name, value);
    }

    public static void Assign(string name, object value)
    {
        if (!Declarations.ContainsKey(name))
        {
            throw new RuntimeException($"Undefined variable named: {name}");
        }

        Declarations[name] = value;
    }

    public static object? GetVariableValue(string name)
    {
        if (Declarations.TryGetValue(name, out var value)) return value;

        throw new RuntimeException($"Undefined variable named: {name}");
    }
}