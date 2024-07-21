namespace CSLox.Parsing.NativeFunctions;

internal static class NativeFunctionsFactory
{
    public static Interpreting.Environment Build()
    {
        var environment = new Interpreting.Environment();
        environment.Decalare("clock", new ClockFunction());
        return environment;
    }
}