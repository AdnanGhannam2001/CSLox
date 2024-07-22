using CSLox.Parsing.Interpreting;
using CSLox.Parsing.Resolving;
using CSLox.Scanning;

namespace CSLox.Parsing.Grammar;

public record VarDeclaration(Token Name, Expr? Initializer = null) : Statement
{
    public override void Resolve()
    {
        Resolver.Declare(Name.Lexeme);
        if (Initializer is not null)
        {
            Initializer.Resolve();
        }
        Resolver.Define(Name.Lexeme);
    }

    public override void Interpret()
    {
        Interpreter.Environment.Declare(Name.Lexeme, Initializer?.Interpret());
    }
}