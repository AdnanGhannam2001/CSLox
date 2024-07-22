using CSLox.Core.Common;
using CSLox.Core.Exceptions;
using CSLox.Parsing;
using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox;

public static class Runner
{
    private static void Run(string source)
    {
        // Lexing
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();

        // Parsing
        var parser = new Parser(tokens);
        var program = parser.Parse();

#if PRINT_TOKENS
        foreach (var token in tokens)
        {
            Logger.LogMessage(token.ToString());
        }
#endif

        // Resolving
        Resolver.Resolve(program);

        // Interpreting
        Interpreter.Interpret(program);
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
            Logger.LogFatal("Failed to load script", exp.Message);
            return;
        }
    }

    public static void RunPrompt()
    {
        ReadLine.HistoryEnabled = true;
        string? input;

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("(prompt)> ");
            Console.ResetColor();

            input = ReadLine.Read();

            try
            {
                Run(input);
            }
            catch (LoxException exp)
            {
                Logger.LogError(exp.Message, exp.Details);
            }
        }
    }
}