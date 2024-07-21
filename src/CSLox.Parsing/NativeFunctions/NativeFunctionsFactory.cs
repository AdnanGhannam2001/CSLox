namespace CSLox.Parsing.NativeFunctions;

internal static class NativeFunctionsFactory
{
    public static Parser.Environment Build()
    {
        var environment = new Parser.Environment();

        environment.Decalare("clock", new ClockFunction());

        return environment;
    }
}