namespace CSLox.Parsing.NativeFunctions;

internal static class NativeFunctionsFactory
{
    public static Interpreting.Environment Build()
    {
        var environment = new Interpreting.Environment();
        environment.Declare("clock", new ClockFunction());
        environment.Declare("read", new ReadFunction());
        environment.Declare("readfile", new ReadFileFunction());
        return environment;
    }
}