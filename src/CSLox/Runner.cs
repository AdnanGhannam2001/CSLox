using CSLox.Core.Common;
using CSLox.Core.Exceptions;
using CSLox.Scanning;

namespace CSLox;

public static class Runner
{
    private static void Run(string source)
    {
        // Lexing
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();

#if DEBUG
        foreach (var token in tokens)
        {
            Logger.LogMessage(token.ToString());
        }
#endif
    }

    public static void RunFile(string path)
    {
        try
        {
            var content = File.ReadAllText(path);
            Run(content);
        }
        catch (FileNotFoundException exp)
        {
            throw new LoxException("Failed to load script", exp.Message);
        }
    }

    public static void RunPrompt()
    {
        string? input;

        do
        {
            Console.Write("> ");
            input = Console.ReadLine();
            Run(input!);
        }
        while (!string.IsNullOrWhiteSpace(input));
    }
}